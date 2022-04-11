using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class QuestTypeService : IQuestTypeService
{
    private readonly IQuestTypeRepository _questTypeRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<QuestType> _sortHelper;
    private readonly IDataShaper<QuestType> _dataShaper;

    public QuestTypeService(IMapper mapper, ISortHelper<QuestType> sortHelper, IDataShaper<QuestType> dataShaper, IQuestTypeRepository questTypeRepository)
    {
        _mapper = mapper;
        _sortHelper = sortHelper;
        _dataShaper = dataShaper;
        _questTypeRepository = questTypeRepository;
    }

    public PageList<Entity> GetAll(QuestTypeParams @params)
    {
        var listAll = _questTypeRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var shapedData = _dataShaper.ShapeData(sortedQuests, @params.Fields);

        return PageList<Entity>.ToPageList(shapedData, @params.PageNume, @params.PageSize);
    }

    public async Task<QuestTypeResponseModel> Get(int id, string? fields)
    {
        var entity = await _questTypeRepository.Get(id);

        //var shaped = _dataShaper.ShapeData(entity, fields);

        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> CreateAsync(QuestTypeRequestModel request)
    {
        var entity = _mapper.Map<QuestType>(request);
        entity = await _questTypeRepository.Add(entity);
        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> UpdateAsync(QuestTypeRequestModel request)
    {
        var entity = _mapper.Map<QuestType>(request);
        entity = await _questTypeRepository.Update(entity);
        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    public async Task<QuestTypeResponseModel> DeleteAsync(int id)
    {
        var entity = await _questTypeRepository.Delete(id);
        return _mapper.Map<QuestTypeResponseModel>(entity);
    }

    private static void Search(ref IQueryable<QuestType> entities, QuestTypeParams param)
    {
        if (!entities.Any() || string.IsNullOrWhiteSpace(param.Name) && string.IsNullOrWhiteSpace(param.Status)) return;

        entities = entities.Where(r => r.Name!.Contains(param.Name!) || r.Status!.Contains(param.Status!));
    }
}