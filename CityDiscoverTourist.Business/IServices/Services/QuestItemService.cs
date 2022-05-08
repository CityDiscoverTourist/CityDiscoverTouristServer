using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class QuestItemService: IQuestItemService
{
    private readonly IQuestItemRepository _taskRepository;
    private readonly IMapper _mapper;
    private readonly ISortHelper<QuestItem> _sortHelper;

    public QuestItemService(IQuestItemRepository taskRepository, IMapper mapper, ISortHelper<QuestItem> sortHelper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<QuestItemResponseModel> GetAll(TaskParams @params)
    {
        var listAll = _taskRepository.GetAll();
        //Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<QuestItemResponseModel>>(sortedQuests);

        return PageList<QuestItemResponseModel>.ToPageList(mappedData, @params.PageNume, @params.PageSize);
    }

    public async Task<QuestItemResponseModel> Get(int id)
    {
        var entity = await _taskRepository.Get(id);

        return _mapper.Map<QuestItemResponseModel>(entity);
    }

    public async Task<QuestItemResponseModel> CreateAsync(QuestItemRequestModel request)
    {
        request.ItemId ??= null;
        var entity = _mapper.Map<QuestItem>(request);

        entity = await _taskRepository.Add(entity);
        return _mapper.Map<QuestItemResponseModel>(entity);
    }

    public async Task<QuestItemResponseModel> UpdateAsync(QuestItemRequestModel request)
    {
        request.ItemId ??= null;
        var entity = _mapper.Map<QuestItem>(request);
        entity = await _taskRepository.Update(entity);
        return _mapper.Map<QuestItemResponseModel>(entity);
    }

    public async Task<QuestItemResponseModel> DeleteAsync(int id)
    {
        var entity = await _taskRepository.Delete(id);
        return _mapper.Map<QuestItemResponseModel>(entity);
    }

    public int CountTaskInQuest(Guid questId)
    {
        throw new NotImplementedException();
    }

    /*private static void Search(ref IQueryable<QuestItem> entities, TaskParams param)
    {
        if (!entities.Any() || string.IsNullOrWhiteSpace(param.Name) && string.IsNullOrWhiteSpace(param.Description)) return;

        entities = entities.Where(x => x.Description!.Contains(param.Description!));
    }

    public int CountTaskInQuest(Guid questId)
    {
        var listAll = _taskRepository.GetAll();
        var count = listAll.Count(r => r.QuestId.Equals(questId));
        return count;
    }*/
}