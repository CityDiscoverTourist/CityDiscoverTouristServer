using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CityDiscoverTourist.Business.IServices.Services;

public class QuestItemTypeService : BaseService, IQuestItemTypeService
{
    private readonly IMapper _mapper;
    private readonly IQuestItemRepository _questItemRepository;
    private readonly IQuestItemTypeRepository _questItemTypeRepository;
    private readonly ISortHelper<QuestItemType> _sortHelper;

    public QuestItemTypeService(IQuestItemTypeRepository questItemTypeRepository, IMapper mapper,
        ISortHelper<QuestItemType> sortHelper, IQuestItemRepository questItemRepository)
    {
        _questItemTypeRepository = questItemTypeRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
        _questItemRepository = questItemRepository;
    }

    public PageList<QuestItemTypeResponseModel> GetAll(TaskTypeParams @params)
    {
        var listAll = _questItemTypeRepository.GetAll().Include(x => x.QuestItems).AsNoTracking();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<QuestItemTypeResponseModel>>(sortedQuests);
        return PageList<QuestItemTypeResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<QuestItemTypeResponseModel> Get(int id, string? fields)
    {
        var entity = await _questItemTypeRepository.Get(id);
        CheckDataNotNull("QuestItemType", entity);
        return _mapper.Map<QuestItemTypeResponseModel>(entity);
    }

    public async Task<QuestItemTypeResponseModel> CreateAsync(QuestItemTypeRequestModel request)
    {
        request.Validate();
        var entity = _mapper.Map<QuestItemType>(request);
        entity = await _questItemTypeRepository.Add(entity);
        return _mapper.Map<QuestItemTypeResponseModel>(entity);
    }

    public async Task<QuestItemTypeResponseModel> UpdateAsync(QuestItemTypeRequestModel request)
    {
        request.Validate();
        var entity = _mapper.Map<QuestItemType>(request);
        entity = await _questItemTypeRepository.Update(entity);
        return _mapper.Map<QuestItemTypeResponseModel>(entity);
    }

    public async Task<QuestItemTypeResponseModel> DeleteAsync(int id)
    {
        var entity = await _questItemTypeRepository.Get(id);
        entity.Status = CommonStatus.Deleted.ToString();
        await _questItemTypeRepository.UpdateFields(entity, r => r.Status!);
        return _mapper.Map<QuestItemTypeResponseModel>(entity);
    }

    public async Task<QuestItemTypeResponseModel> DisableAsync(int id)
    {
        var entity = await _questItemTypeRepository.Get(id);
        entity.Status = CommonStatus.Inactive.ToString();
        await _questItemTypeRepository.UpdateFields(entity, r => r.Status!);
        return _mapper.Map<QuestItemTypeResponseModel>(entity);
    }

    public async Task<QuestItemTypeResponseModel> EnableAsync(int id)
    {
        var entity = await _questItemTypeRepository.Get(id);
        entity.Status = CommonStatus.Active.ToString();
        await _questItemTypeRepository.UpdateFields(entity, r => r.Status!);
        return _mapper.Map<QuestItemTypeResponseModel>(entity);
    }

    public async Task<QuestItemTypeResponseModel> UpdateStatusForeignKey(int id, string status)
    {
        var includedEntity = _questItemRepository.GetByCondition(x => x.QuestItemTypeId == id).ToList();
        foreach (var area in includedEntity)
        {
            area.Status = status;
            await _questItemRepository.UpdateFields(area, r => r.Status!);
        }

        return null!;
    }

    private static void Search(ref IQueryable<QuestItemType> entities, TaskTypeParams param)
    {
        if (!entities.Any()) return;

        if (param.Status != null) entities = entities.Where(x => x.Status == param.Status);
        if (param.Name != null) entities = entities.Where(x => x.Name!.Contains(param.Name));
    }
}