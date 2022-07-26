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
using Newtonsoft.Json.Linq;

namespace CityDiscoverTourist.Business.IServices.Services;

public class QuestService : BaseService, IQuestService
{
    private readonly IBlobService _blobService;
    private readonly ICustomerQuestRepository _customerQuestRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;
    private readonly IQuestItemRepository _questItemRepository;
    private readonly IQuestRepository _questRepository;
    private readonly ISortHelper<Quest> _sortHelper;
    private readonly INotificationService _notificationService;
    private readonly IAreaRepository _areaRepository;

    public QuestService(IQuestRepository questRepository, ISortHelper<Quest> sortHelper, IMapper mapper,
        IBlobService blobService, ILocationRepository locationRepository, IQuestItemRepository questItemRepository,
        ICustomerQuestRepository customerQuestRepository, INotificationService notificationService, IAreaRepository areaRepository)
    {
        _questRepository = questRepository;
        _sortHelper = sortHelper;
        _mapper = mapper;
        _blobService = blobService;
        _locationRepository = locationRepository;
        _questItemRepository = questItemRepository;
        _customerQuestRepository = customerQuestRepository;
        _notificationService = notificationService;
        _areaRepository = areaRepository;
    }


    public PageList<QuestResponseModel> GetAll(QuestParams param, Language language)
    {
        var listAll = _questRepository.GetAll().AsNoTracking();

        Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, param.OrderBy);

        var listQuestIds = sortedQuests.Select(x => x.Id).ToList();
        var listQuest = sortedQuests.ToList();

        // set include quest item for quest
        for (var i = 0; i < listQuestIds.Count; i++)
        {
            var quest = listQuest[i];
            var questItems = _questItemRepository.GetAll().AsNoTracking().Where(x => x.QuestId == quest.Id).ToList();

            // convert quest item between vi and en
            foreach (var questItem in questItems)
            {
                questItem.Content = ConvertLanguage(language, questItem.Content!);
                questItem.Description = ConvertLanguage(language, questItem.Description!);
            }

            listQuest[i].QuestItems = questItems;
        }

        var mappedData = _mapper.Map<IEnumerable<QuestResponseModel>>(listQuest);

        var questResponseModels = mappedData as QuestResponseModel[] ?? mappedData.ToArray();

        for (var i = 0; i < questResponseModels.Length; i++)
        {
            //get total feed back and average rate for each quest
            var i1 = i;
            var customerQuest = _customerQuestRepository.GetByCondition(x => x.QuestId == questResponseModels[i1].Id);

            if (customerQuest.Any())
            {
                questResponseModels[i].AverageStar = (long?) customerQuest.Average(x => x.Rating);
                questResponseModels[i].TotalFeedback = customerQuest.Count();
            }
            else
            {
                questResponseModels[i].AverageStar = 0;
                questResponseModels[i].TotalFeedback = 0;
            }

            var areaId = questResponseModels[i].AreaId;
            questResponseModels[i].AreaName = _areaRepository.Get(areaId).Result.Name ?? "K co";

            // count quest item for each quest
            for (var j = 0; j < questResponseModels[i].QuestItems!.Count; j++)
            {
                var questItem = questResponseModels[i].QuestItems![j];
                if (questItem.ItemId != 0) continue;

                var locationId = questItem.LocationId;
                var location = _locationRepository.Get(locationId).Result;
                questResponseModels[i].Address = location.Address;
                questResponseModels[i].LatLong = location.Latitude + "," + location.Longitude;
            }

            questResponseModels[i].Title = ConvertLanguage(language, questResponseModels[i].Title!);
            questResponseModels[i].Description = ConvertLanguage(language, questResponseModels[i].Description!);

            var quest = questResponseModels[i].QuestItems!.Count;
            questResponseModels[i].CountQuestItem = quest;
        }

        return PageList<QuestResponseModel>.ToPageList(questResponseModels, param.PageNumber, param.PageSize);
    }

    public async Task<QuestResponseModel> Get(int id, Language language)
    {
        var entity = await _questRepository.GetByCondition(x => x.Id == id).Include(x => x.QuestItems)
            .FirstOrDefaultAsync();

        CheckDataNotNull("Quest", entity!);

        var questItems = entity!.QuestItems!.ToList();

        foreach (var questItem in questItems)
        {
            questItem.Content = ConvertLanguage(language, questItem.Content!);
            questItem.Description = ConvertLanguage(language, questItem.Description!);
        }

        var title = ConvertLanguage(language, entity.Title!);
        var description = ConvertLanguage(language, entity.Description!);

        entity.Title = title;
        entity.Description = description;

        var mappedData = _mapper.Map<QuestResponseModel>(entity);

        //get total feed back and average rate for each quest
        var customerQuests = _customerQuestRepository.GetByCondition(x => x.QuestId == id);

        if (customerQuests.Any())
        {
            var averageRating = customerQuests.Average(x => x.Rating);

            mappedData.AverageStar = (long) averageRating;
            mappedData.TotalFeedback = customerQuests.Count();
        }
        else
        {
            mappedData.AverageStar = 0;
            mappedData.TotalFeedback = 0;
        }

        foreach (var item in mappedData.QuestItems!)
        {
            if (item.ItemId != 0) continue;
            var locationId = item.LocationId;
            var location = _locationRepository.Get(locationId).Result;
            mappedData.Address = location.Address;
            mappedData.LatLong = location.Latitude + "," + location.Longitude;
        }

        mappedData.CountQuestItem = mappedData.QuestItems!.Count;

        return mappedData;
    }


    public async Task<QuestResponseModel> Get(int id)
    {
        var entity = await _questRepository.GetByCondition(x => x.Id == id).FirstOrDefaultAsync();

        CheckDataNotNull("Quest", entity!);

        var objTitle = JObject.Parse(entity!.Title!);
        var title = (string) objTitle["vi"]! + " | " + (string) objTitle["en"]!;
        var objDescription = JObject.Parse(entity.Description!);
        var description = (string) objDescription["vi"]! + " | " + (string) objDescription["en"]!;

        entity.Title = title;
        entity.Description = description;

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

        return mappedData;
    }

    public async Task<QuestResponseModel> CreateAsync(QuestRequestModel request)
    {
        request.Validate();
        var existValue = _questRepository.GetByCondition(x => request.Title == x.Title).FirstOrDefaultAsync().Result;
        if (existValue != null) throw new AppException("Quest with this name already exists");

        var entity = _mapper.Map<Quest>(request);

        entity.Title = JsonHelper.JsonFormat(request.Title);
        entity.Description = JsonHelper.JsonFormat(request.Description);

        entity = await _questRepository.Add(entity);

        //create notification for quest created and push to hub
        await _notificationService.CreateAsync(new Notification
        {
            Content = "New quest " + ConvertLanguage(Language.vi, entity.Title!) + " has been created",
            CreatedDate = CurrentDateTime()
        });

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

        entity.Title = JsonHelper.JsonFormat(request.Title);
        entity.Description = JsonHelper.JsonFormat(request.Description);
        entity.UpdatedDate = CurrentDateTime();
        entity.ImagePath = imgPath;

        if (entity.ImagePath == null)
            entity = await _questRepository.NoneUpdateFields(entity, r => r.CreatedDate!, r => r.ImagePath!);

        entity = await _questRepository.NoneUpdateFields(entity, r => r.CreatedDate!);

        return _mapper.Map<QuestResponseModel>(entity);
    }

    public async Task<QuestResponseModel> DeleteAsync(int questId)
    {
        var entity = _questRepository.GetByCondition(x => x.Id == questId).Include(data => data.QuestItems).ToList()
            .FirstOrDefault();
        if (entity != null && entity.QuestItems!.Count == 0)
        {
            entity.Status = CommonStatus.Inactive.ToString();
            await _questRepository.UpdateFields(entity, r => r.Status!);
        }

        return _mapper.Map<QuestResponseModel>(entity);
    }

    public async Task<QuestResponseModel> DisableAsync(int questId)
    {
        var entity = _questRepository.GetByCondition(x => x.Id == questId)
            .Include(data => data.QuestItems).ToList().FirstOrDefault();

        if(entity == null || entity.QuestItems!.Count != 0) return _mapper.Map<QuestResponseModel>(entity);

        entity!.Status = CommonStatus.Inactive.ToString();
        await _questRepository.UpdateFields(entity, r => r.Status!);

        return _mapper.Map<QuestResponseModel>(entity);
    }

    public async Task<QuestResponseModel> EnableAsync(int questId)
    {
        var entity = await _questRepository.Get(questId);
        entity.Status = CommonStatus.Active.ToString();
        await _questRepository.UpdateFields(entity, r => r.Status!);
        return _mapper.Map<QuestResponseModel>(entity);
    }

    public async Task<QuestResponseModel> UpdateStatusForeignKey(int questId, string status)
    {
        var includedEntity = _questItemRepository.GetByCondition(x => x.QuestId == questId).ToList();
        foreach (var area in includedEntity)
        {
            area.Status = status;
            await _questItemRepository.UpdateFields(area, r => r.Status!);
        }

        return null!;
    }

    private static void Search(ref IQueryable<Quest> entities, QuestParams param)
    {
        if (!entities.Any()) return;

        if (param.Name != null) entities = entities.Where(r => r.Title!.Contains(param.Name));
        if (param.Description != null) entities = entities.Where(r => r.Description!.Contains(param.Description));
        if (param.Status != null) entities = entities.Where(r => r.Status!.Equals(param.Status));
        if (param.QuestTypeId != 0) entities = entities.Where(r => r.QuestTypeId.Equals(param.QuestTypeId));
        if (param.AreaId != 0) entities = entities.Where(r => r.AreaId.Equals(param.AreaId));
    }
}