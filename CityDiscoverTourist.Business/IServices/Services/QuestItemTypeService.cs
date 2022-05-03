using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class QuestItemTypeService : IQuestItemTypeService
{
    private readonly IQuestItemTypeRepository _questItemTypeRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<QuestItemType> _sortHelper;

    public QuestItemTypeService(IQuestItemTypeRepository questItemTypeRepository, IMapper mapper, ISortHelper<QuestItemType> sortHelper)
    {
        _questItemTypeRepository = questItemTypeRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<QuestItemTypeResponseModel> GetAll(TaskTypeParams @params)
    {
        var listAll = _questItemTypeRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<QuestItemTypeResponseModel>>(sortedQuests);
        return PageList<QuestItemTypeResponseModel>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<QuestItemTypeResponseModel> Get(int id, string? fields)
    {
        var entity = await _questItemTypeRepository.Get(id);

        return _mapper.Map<QuestItemTypeResponseModel>(entity);
    }

    public async Task<QuestItemTypeResponseModel> CreateAsync(QuestItemTypeRequestModel request)
    {
        var entity = _mapper.Map<QuestItemType>(request);
        entity = await _questItemTypeRepository.Add(entity);
        return _mapper.Map<QuestItemTypeResponseModel>(entity);
    }

    public async Task<QuestItemTypeResponseModel> UpdateAsync(QuestItemTypeRequestModel request)
    {
        var entity = _mapper.Map<QuestItemType>(request);
        entity = await _questItemTypeRepository.Update(entity);
        return _mapper.Map<QuestItemTypeResponseModel>(entity);
    }

    public async Task<QuestItemTypeResponseModel> DeleteAsync(int id)
    {
        var entity = await _questItemTypeRepository.Delete(id);
        return _mapper.Map<QuestItemTypeResponseModel>(entity);
    }

    private static void Search(ref IQueryable<QuestItemType> entities, TaskTypeParams param)
    {
        if (!entities.Any() || string.IsNullOrWhiteSpace(param.Name) && string.IsNullOrWhiteSpace(param.Status)) return;

        //entities = entities.Where(r => r.Name!.Contains(param.Name!));
    }
}