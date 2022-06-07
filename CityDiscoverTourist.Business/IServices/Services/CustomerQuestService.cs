using System.Globalization;
using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Quest = CityDiscoverTourist.Data.Models.Quest;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CustomerQuestService: BaseService, ICustomerQuestService
{
    private readonly ICustomerQuestRepository _customerQuestRepository;
    private readonly IQuestItemRepository _taskRepository;
    private readonly ICustomerTaskService _customerTaskService;
    private readonly IMapper _mapper;
    private readonly ISortHelper<CustomerQuest> _sortHelper;
    private const int BaseMultiplier = 150;

    public CustomerQuestService(ICustomerQuestRepository customerQuestRepository, IMapper mapper, IQuestItemRepository taskRepository, ISortHelper<CustomerQuest> sortHelper, ICustomerTaskService customerTaskService)
    {
        _customerQuestRepository = customerQuestRepository;
        _mapper = mapper;
        _taskRepository = taskRepository;
        _sortHelper = sortHelper;
        _customerTaskService = customerTaskService;
    }

    public PageList<CustomerQuestResponseModel> GetAll(CustomerQuestParams @params)
    {
        var listAll = _customerQuestRepository.GetAll();

        Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);

        var mappedData = _mapper.Map<IEnumerable<CustomerQuestResponseModel>>(sortedQuests);
        return PageList<CustomerQuestResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }

    public async Task<CustomerQuestResponseModel> Get(int id)
    {
        var entity = await _customerQuestRepository.Get(id);
        CheckDataNotNull("CustomerQuest", entity);
        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    public async Task<CustomerQuestResponseModel> CreateAsync(CustomerQuestRequestModel request)
    {
        var entity = _mapper.Map<CustomerQuest>(request);

        entity.IsFinished = false;
        entity.BeginPoint = CalculateBeginPoint(request.QuestId);

        entity = await _customerQuestRepository.Add(entity);
        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    public async Task<CustomerQuestResponseModel> UpdateAsync(CustomerQuestRequestModel request)
    {
        var entity = _mapper.Map<CustomerQuest>(request);
        entity = await _customerQuestRepository.Update(entity);
        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    public async Task<CustomerQuestResponseModel> DeleteAsync(int id)
    {
        var entity = await _customerQuestRepository.Delete(id);
        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    public async Task<CustomerQuestResponseModel> UpdateEndPointAndStatusWhenFinishQuestAsync(int id)
    {
        var isLastItem = await _customerTaskService.IsLastQuestItem(id);

        if (!isLastItem) throw new AppException("Quest is not finished");

        var lastPoint = _customerTaskService.GetLastPoint(id);
        var entity = await  _customerQuestRepository.Get(id);

        entity.EndPoint = lastPoint.ToString(CultureInfo.InvariantCulture);
        entity.Status = CommonStatus.Done.ToString();
        entity.IsFinished = true;
        entity = await _customerQuestRepository.UpdateFields(entity, x => x.EndPoint!,
            x => x.Status!, x => x.IsFinished);

        return _mapper.Map<CustomerQuestResponseModel>(entity);
    }

    private static void Search(ref IQueryable<CustomerQuest> entities, CustomerQuestParams param)
    {
        if (!entities.Any()) return;

        if (param.QuestId != 0)
            entities = entities.Where(x => x.QuestId == param.QuestId);
    }

    private int CountQuestItemInQuest(int questId)
    {
        var questItems = _taskRepository.GetByCondition(x => x.QuestId == questId);
        return questItems.Count();
    }

    private string CalculateBeginPoint(int questId)
    {
        return (CountQuestItemInQuest(questId) * BaseMultiplier).ToString();
    }
}