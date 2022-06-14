using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CityDiscoverTourist.Business.IServices.Services;

public class QuestService : BaseService, IQuestService
{
    private readonly IBlobService _blobService;
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;
    private readonly IQuestRepository _questRepository;
    private readonly ISortHelper<Quest> _sortHelper;

    public QuestService(IQuestRepository questRepository, ISortHelper<Quest> sortHelper, IMapper mapper,
        IBlobService blobService, ILocationRepository locationRepository)
    {
        _questRepository = questRepository;
        _sortHelper = sortHelper;
        _mapper = mapper;
        _blobService = blobService;
        _locationRepository = locationRepository;
    }


    public PageList<QuestResponseModel> GetAll(QuestParams param)
    {
        var listAll = _questRepository.GetAll().Include(x => x.QuestItems).AsNoTracking();

        Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, param.OrderBy);

        var mappedData = _mapper.Map<IEnumerable<QuestResponseModel>>(sortedQuests);
        // count quest item for each quest
        var questResponseModels = mappedData as QuestResponseModel[] ?? mappedData.ToArray();

        for (var i = 0; i < questResponseModels.Length; i++)
        {
            for (var j = 0; j < questResponseModels[i].QuestItems!.Count; j++)
            {
                var questItem = questResponseModels[i].QuestItems![j];
                if (questItem.ItemId != 0) continue;

                var locationId = questItem.LocationId;
                var location = _locationRepository.Get(locationId).Result;
                questResponseModels[i].Address = location.Address;
                questResponseModels[i].LatLong = location.Latitude + "," + location.Longitude;
            }

            var quest = questResponseModels[i].QuestItems!.Count;
            questResponseModels[i].CountQuestItem = quest;
        }

        return PageList<QuestResponseModel>.ToPageList(questResponseModels, param.PageNumber, param.PageSize);
    }

    public async Task<QuestResponseModel> Get(int id)
    {
        var entity = await _questRepository.GetByCondition(x => x.Id == id).Include(x => x.QuestItems)
            .FirstOrDefaultAsync();
        var mappedData = _mapper.Map<QuestResponseModel>(entity);
        foreach (var item in mappedData.QuestItems!)
        {
            if (item.ItemId != 0) continue;
            var locationId = item.LocationId;
            var location = _locationRepository.Get(locationId).Result;
            mappedData.Address = location.Address;
            mappedData.LatLong = location.Latitude + "," + location.Longitude;
        }

        mappedData.CountQuestItem = mappedData.QuestItems!.Count;
        CheckDataNotNull("Quest", entity!);
        return mappedData;
    }

    public async Task<QuestResponseModel> CreateAsync(QuestRequestModel request)
    {
        request.Validate();
        var existValue = _questRepository.GetByCondition(x => request.Title == x.Title).FirstOrDefaultAsync().Result;
        if (existValue != null) throw new AppException("Quest with this name already exists");

        var entity = _mapper.Map<Quest>(request);
        entity = await _questRepository.Add(entity);
        //return string img from blob, mapped to Quest model and store in db
        var imgPath = await _blobService.UploadQuestImgAndReturnImgPathAsync(request.Image, entity.Id, "quest");
        entity.ImagePath = imgPath;
        await _questRepository.UpdateFields(entity, r => r.ImagePath!);
        return _mapper.Map<QuestResponseModel>(entity);
    }

    public async Task<QuestResponseModel> UpdateAsync(QuestRequestModel request)
    {
        request.Validate();

        var imgPath = await _blobService.UploadQuestImgAndReturnImgPathAsync(request.Image, request.Id, "quest");

        var entity = _mapper.Map<Quest>(request);
        entity.ImagePath = imgPath;
        if (entity.ImagePath == null)
        {
            entity = await _questRepository.NoneUpdateFields(entity, r => r.CreatedDate!, r => r.ImagePath!);
            return _mapper.Map<QuestResponseModel>(entity);
        }

        entity = await _questRepository.NoneUpdateFields(entity, r => r.CreatedDate!);

        return _mapper.Map<QuestResponseModel>(entity);
    }

    public async Task<QuestResponseModel> DeleteAsync(int questId)
    {
        var entity = await _questRepository.Get(questId);
        entity.Status = CommonStatus.Deleted.ToString();
        await _questRepository.UpdateFields(entity, r => r.Status!);
        return _mapper.Map<QuestResponseModel>(entity);
    }

    private static void Search(ref IQueryable<Quest> entities, QuestParams param)
    {
        if (!entities.Any()) return;

        if (param.Name != null) entities = entities.Where(r => r.Title!.Contains(param.Name));
        if (param.Description != null) entities = entities.Where(r => r.Description!.Contains(param.Description));
        if (param.Status != null) entities = entities.Where(r => r.Status!.Contains(param.Status));
        if (param.QuestTypeId != 0) entities = entities.Where(r => r.QuestTypeId.Equals(param.QuestTypeId));
    }
}