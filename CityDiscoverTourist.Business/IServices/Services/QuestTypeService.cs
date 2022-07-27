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

public class QuestTypeService : BaseService, IQuestTypeService
{
    private readonly IBlobService _blobService;
    private readonly ICustomerQuestRepository _customerQuestRepository;
    private readonly IMapper _mapper;
    private readonly IQuestRepository _questRepository;
    private readonly IQuestTypeRepository _questTypeRepository;
    private readonly ISortHelper<QuestType> _sortHelper;

    public QuestTypeService(IMapper mapper, ISortHelper<QuestType> sortHelper, IQuestTypeRepository questTypeRepository,
        IBlobService blobService, IQuestRepository questRepository, ICustomerQuestRepository customerQuestRepository)
    {
        _mapper = mapper;
        _sortHelper = sortHelper;
        _questTypeRepository = questTypeRepository;
        _blobService = blobService;
        _questRepository = questRepository;
        _customerQuestRepository = customerQuestRepository;
    }

    public PageList<QuestTypeResponseModel> GetAll(QuestTypeParams @params, Language language)
    {
        var listAll = _questTypeRepository.GetAll().AsNoTracking();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);

        var listQuestTypeIds = sortedQuests.Select(x => x.Id).ToList();
        var listQuestType = _mapper.Map<List<QuestTypeResponseModel>>(sortedQuests);

        for (var i = 0; i < listQuestTypeIds.Count; i++)
        {
            var questType = listQuestType[i];
            var quests = _questRepository.GetAll().AsNoTracking().Where(x => x.QuestTypeId == questType.Id).ToList();

            var mappedQuest = _mapper.Map<List<QuestResponseModel>>(quests);

            // convert quest item between vi and en
            foreach (var item in mappedQuest)
            {
                //get total feed back and average rate for each quest
                var customerQuests = _customerQuestRepository.GetAll().AsNoTracking().Where(x => x.QuestId == item.Id)
                    .ToList();

                if (customerQuests.Any())
                {
                    item.TotalFeedback = customerQuests.Count;
                    item.AverageStar = (long?) customerQuests.Average(x => x.Rating);
                }
                else
                {
                    item.TotalFeedback = 0;
                    item.AverageStar = 0;
                }

                item.Title = ConvertLanguage(language, item.Title!);
                item.Description = ConvertLanguage(language, item.Description!);
            }

            listQuestType[i].Quests = mappedQuest;
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
        var mappedQuest = _mapper.Map<List<QuestResponseModel>>(quests);

        // convert quest item between vi and en
        foreach (var item in mappedQuest)
        {
            //get total feed back and average rate for each quest
            var customerQuests = _customerQuestRepository.GetByCondition(x => x.QuestId == item.Id);

            if (customerQuests.Any())
            {
                item.TotalFeedback = customerQuests.Count();
                item.AverageStar = (long?) customerQuests.Average(x => x.Rating);
            }
            else
            {
                item.TotalFeedback = 0;
                item.AverageStar = 0;
            }

            item.Title = ConvertLanguage(language, item.Title!);
            item.Description = ConvertLanguage(language, item.Description!);
        }

        var mappedData = _mapper.Map<QuestTypeResponseModel>(entity);

        mappedData.Name = ConvertLanguage(language, entity.Name!);
        mappedData.Quests = mappedQuest;

        return mappedData;
    }

    public async Task<QuestTypeResponseModel> Get(int id)
    {
        var entity = await _questTypeRepository.GetByCondition(x => x.Id == id).FirstOrDefaultAsync();
        CheckDataNotNull("QuestType", entity!);

        var objName = JObject.Parse(entity!.Name!);
        var name = (string) objName["vi"]! + " | " + (string) objName["en"]!;

        entity.Name = name;

        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> CreateAsync(QuestTypeRequestModel request)
    {
        request.Validate();
        var requestName = GetVietNameseName(request.Name!);

        var existValue = _questTypeRepository.GetAll();
        foreach (var exist in existValue)
        {
            if (Trim(ConvertLanguage(Language.vi, exist.Name)) == Trim(requestName))
            {
                throw new AppException("QuestType name is exist");
            }
        }

        var entity = _mapper.Map<QuestType>(request);
        entity = await _questTypeRepository.Add(entity);

        var imgPath = await _blobService.UploadQuestImgAndReturnImgPathAsync(request.Image, entity.Id, "quest-type");
        entity.ImagePath = imgPath;

        entity.Name = JsonHelper.JsonFormat(request.Name);

        await _questTypeRepository.UpdateFields(entity, r => r.ImagePath!);

        //return quest type name vi
        var objTitle = JObject.Parse(entity!.Name!);
        var name = (string) objTitle["vi"]!;
        entity.Name = name;

        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> UpdateAsync(QuestTypeRequestModel request)
    {
        request.Validate();

        var imgPath = await _blobService.UploadQuestImgAndReturnImgPathAsync(request.Image, request.Id, "quest-type");

        var entity = _mapper.Map<QuestType>(request);
        entity.Name = JsonHelper.JsonFormat(request.Name);

        entity.ImagePath = imgPath;
        if (entity.ImagePath == null)
        {
            entity = await _questTypeRepository.NoneUpdateFields(entity, r => r.Id, r => r.ImagePath!);
            return _mapper.Map<QuestTypeResponseModel>(entity);
        }

        entity = await _questTypeRepository.NoneUpdateFields(entity, r => r.Id);

        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> DeleteAsync(int id)
    {
        var entity = _questTypeRepository.GetByCondition(x => x.Id == id).Include(data => data.Quests).ToList()
            .FirstOrDefault();
        if (entity != null && entity.Quests!.Count == 0)
        {
            entity.Status = CommonStatus.Inactive.ToString();
            await _questTypeRepository.UpdateFields(entity, r => r.Status!);
        }

        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> DisableAsync(int id)
    {
        var entity = _questTypeRepository.GetByCondition(x => x.Id == id)
            .Include(data => data.Quests).ToList().FirstOrDefault();

        if(entity == null || entity.Quests!.Count != 0) return _mapper.Map<QuestTypeResponseModel>(entity);

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