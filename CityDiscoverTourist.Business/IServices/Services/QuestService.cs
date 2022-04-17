using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class QuestService: IQuestService
{
    private readonly IQuestRepository _questRepository;
    private readonly ISortHelper<Quest> _sortHelper;
    private readonly IDataShaper<Quest> _dataShaper;
    private readonly IMapper _mapper;

    public QuestService(IQuestRepository questRepository, ISortHelper<Quest> sortHelper, IDataShaper<Quest> dataShaper, IMapper mapper)
    {
        _questRepository = questRepository;
        _sortHelper = sortHelper;
        _dataShaper = dataShaper;
        _mapper = mapper;
    }


    public PageList<Entity> GetAll(QuestParams param)
    {
        var listAll = _questRepository.GetAll();

        Search(ref listAll, param);

        var sortedQuests = _sortHelper.ApplySort(listAll, param.OrderBy);
        var shapedData = _dataShaper.ShapeData(sortedQuests, param.Fields);

        return PageList<Entity>.ToPageList(shapedData, param.PageNume, param.PageSize);
    }

    public async Task<QuestResponseModel> Get(Guid id, string? fields)
    {
        var entity = await _questRepository.Get(id);

        //var shaped = _dataShaper.ShapeData(entity, fields);

        return _mapper.Map<QuestResponseModel>(entity);
    }

    public async Task<QuestResponseModel> CreateAsync(QuestRequestModel request)
    {
        var entity = _mapper.Map<Quest>(request);
        entity = await _questRepository.Add(entity);
        return _mapper.Map<QuestResponseModel>(entity);
    }

    public async Task<QuestResponseModel> UpdateAsync(QuestRequestModel request)
    {
        var entity = _mapper.Map<Quest>(request);
        entity = await _questRepository.Update(entity);
        return _mapper.Map<QuestResponseModel>(entity);
    }

    public async Task<QuestResponseModel> DeleteAsync(Guid questId)
    {
        var entity = await _questRepository.Delete(questId);
        return _mapper.Map<QuestResponseModel>(entity);
    }


    private static void Search(ref IQueryable<Quest> entities, QuestParams param)
    {
        if (!entities.Any() || string.IsNullOrWhiteSpace(param.Name) && string.IsNullOrWhiteSpace(param.Status)) return;

        entities = entities.Where(r => r.Status!.Contains(param.Status!));
    }
}