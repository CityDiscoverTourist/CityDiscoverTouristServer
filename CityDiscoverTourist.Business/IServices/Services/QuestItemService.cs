using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CityDiscoverTourist.Business.IServices.Services;

public class QuestItemService: BaseService, IQuestItemService
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
        Search(ref listAll, @params);
        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<QuestItemResponseModel>>(sortedQuests);

        return PageList<QuestItemResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<QuestItemResponseModel> Get(int id)
    {
        var entity = await _taskRepository.Get(id);
        CheckDataNotNull("QuestItem", entity);
        return _mapper.Map<QuestItemResponseModel>(entity);
    }

    public async Task<QuestItemResponseModel> CreateAsync(QuestItemRequestModel request)
    {
        var existValue = _taskRepository
            .GetByCondition(x => x.Content == request.Content || x.Content == ReverseQuestion(request.Content!))
            .FirstOrDefaultAsync().Result;
        if (existValue != null) throw new AppException("Quest item with this name already exists");

        // set sequence for new quest item
        var questItems = _taskRepository.GetByCondition(x => x.QuestId == request.QuestId).ToList();
        if (questItems.Count == 0) request.ItemId = null;
        else
        {
            var lastQuestItemId = questItems.Max(x => x.Id);
            request.ItemId = lastQuestItemId;
        }

        // quest item type 3 is ReverseQuestion
        if (request.QuestItemTypeId == 3) request.Content = ReverseQuestion(request.Content!);

        var entity = _mapper.Map<QuestItem>(request);

        entity = await _taskRepository.Add(entity);
        return _mapper.Map<QuestItemResponseModel>(entity);
    }

    public async Task<QuestItemResponseModel> UpdateAsync(QuestItemRequestModel request)
    {
        var existValue = _taskRepository.GetByCondition(x => request.Content == x.Content).FirstOrDefaultAsync().Result;
        if (existValue != null) throw new AppException("Quest item with this name already exists");

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

    public async Task<QuestItemResponseModel> GetByQuestId(int id)
    {
        var entity = _taskRepository.GetByCondition(x => x.QuestId == id).ToList();
        CheckDataNotNull("QuestItem", entity);
        return _mapper.Map<QuestItemResponseModel>(entity);
    }

    private static void Search(ref IQueryable<QuestItem> entities, TaskParams param)
    {
        if (!entities.Any()) return;
        if (param.Name != null)
        {
            entities = entities.Where(x => x.Content!.Contains(param.Name));
        }
        if (param.QuestId != 0)
        {
            entities = entities.Where(x => x.QuestId == param.QuestId);
        }

        if (param.QuestItemTypeId != 0)
        {
            entities = entities.Where(x => x.QuestItemTypeId == param.QuestItemTypeId);
        }
    }

    private static string ReverseQuestion(string question)
    {
        var reversed = "";
        for (var i = question.Length - 1; i >= 0; i--)
        {
            reversed += question[i];
        }
        return reversed;
    }
}