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

public class QuestTypeService : BaseService, IQuestTypeService
{
    private readonly IBlobService _blobService;
    private readonly IMapper _mapper;
    private readonly IQuestRepository _questRepository;
    private readonly IQuestTypeRepository _questTypeRepository;
    private readonly ISortHelper<QuestType> _sortHelper;

    public QuestTypeService(IMapper mapper, ISortHelper<QuestType> sortHelper, IQuestTypeRepository questTypeRepository,
        IBlobService blobService, IQuestRepository questRepository)
    {
        _mapper = mapper;
        _sortHelper = sortHelper;
        _questTypeRepository = questTypeRepository;
        _blobService = blobService;
        _questRepository = questRepository;
    }

    public PageList<QuestTypeResponseModel> GetAll(QuestTypeParams @params, Language language)
    {
        var listAll = _questTypeRepository.GetAll().AsNoTracking();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);

        var listQuestTypeIds = sortedQuests.Select(x => x.Id).ToList();
        var listQuestType = sortedQuests.ToList();

        for (var i = 0; i < listQuestTypeIds.Count; i++)
        {
            var questType = listQuestType[i];
            var quests = _questRepository.GetAll()
                .AsNoTracking()
                .Where(x => x.QuestTypeId == questType.Id)
                .ToList();
            // convert quest item between vi and en
            foreach (var questItem in quests)
            {
                questItem.Title = ConvertLanguage(language, questItem.Title!);
                questItem.Description = ConvertLanguage(language, questItem.Description!);
            }
            listQuestType[i].Quests = quests;
            listQuestType[i].Name = ConvertLanguage(language, questType.Name!);
        }

        var mappedData = _mapper.Map<IEnumerable<QuestTypeResponseModel>>(listQuestType);
        return PageList<QuestTypeResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<QuestTypeResponseModel> Get(int id, Language language)
    {
        var entity = await _questTypeRepository.GetByCondition(x => x.Id == id).Include(x => x.Quests)
            .FirstOrDefaultAsync();
        CheckDataNotNull("QuestType", entity!);

        var quests = entity!.Quests!.ToList();
        // convert quest item between vi and en
        foreach (var item in quests)
        {
            item.Title = ConvertLanguage(language, item.Title!);
            item.Description = ConvertLanguage(language, item.Description!);
        }

        entity.Name = ConvertLanguage(language, entity.Name!);

        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> CreateAsync(QuestTypeRequestModel request)
    {
        request.Validate();
        var existValue = _questTypeRepository.GetByCondition(x => request.Name == x.Name).FirstOrDefaultAsync().Result;
        if (existValue != null) throw new AppException("QuestType with this name already exists");

        var entity = _mapper.Map<QuestType>(request);
        entity = await _questTypeRepository.Add(entity);

        var imgPath = await _blobService.UploadQuestImgAndReturnImgPathAsync(request.Image, entity.Id, "quest-type");
        entity.ImagePath = imgPath;

        entity.Name = JsonHelper.JsonFormat(request.Name);

        await _questTypeRepository.UpdateFields(entity, r => r.ImagePath!);

        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> UpdateAsync(QuestTypeRequestModel request)
    {
        request.Validate();

        var imgPath = await _blobService.UploadQuestImgAndReturnImgPathAsync(request.Image, request.Id, "quest-type");

        var entity = _mapper.Map<QuestType>(request);

        entity.ImagePath = imgPath;
        if (entity.ImagePath == null)
        {
            entity = await _questTypeRepository.NoneUpdateFields(entity, r => r.Id, r => r.ImagePath!);
            return _mapper.Map<QuestTypeResponseModel>(entity);
        }

        entity.Name = JsonHelper.JsonFormat(request.Name);

        entity = await _questTypeRepository.NoneUpdateFields(entity, r => r.Id);

        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> DeleteAsync(int id)
    {
        var entity = await _questTypeRepository.Get(id);
        entity.Status = CommonStatus.Deleted.ToString();
        await _questTypeRepository.UpdateFields(entity, r => r.Status!);
        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> DisableAsync(int id)
    {
        var entity = await _questTypeRepository.Get(id);
        entity.Status = CommonStatus.Inactive.ToString();
        await _questTypeRepository.UpdateFields(entity, r => r.Status!);
        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> EnableAsync(int id)
    {
        var entity = await _questTypeRepository.Get(id);
        entity.Status = CommonStatus.Active.ToString();
        await _questTypeRepository.UpdateFields(entity, r => r.Status!);
        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> UpdateStatusForeignKey(int id, string status)
    {
        var includedEntity = _questRepository.GetByCondition(x => x.QuestTypeId == id).ToList();
        foreach (var area in includedEntity)
        {
            area.Status = status;
            await _questRepository.UpdateFields(area, r => r.Status!);
        }
        return null!;
    }

    public  Task<int> CountQuestInQuestType(int questTypeId)
    {
        return Task.FromResult(_questRepository.GetAll().Count(x => x.QuestTypeId == questTypeId));
    }

    private static void Search(ref IQueryable<QuestType> entities, QuestTypeParams param)
    {
        if (!entities.Any()) return;

        if (param.Status != null) entities = entities.Where(x => x.Status == param.Status);
        if (param.Name != null) entities = entities.Where(x => x.Name!.Contains(param.Name));
    }
}