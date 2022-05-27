using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CustomerTaskService: BaseService, ICustomerTaskService
{
    private readonly ICustomerTaskRepository _customerTaskRepo;
    private readonly IMapper _mapper;
    private readonly ISortHelper<CustomerTask> _sortHelper;
    private readonly ICustomerQuestRepository _customerQuestRepo;
    private readonly IQuestItemRepository _questItemRepo;
    private const int PointWhenHitSuggestion = 150;
    private const int PointWhenWrongAnswer = 100;

    public CustomerTaskService(ICustomerTaskRepository customerTaskRepository, IMapper mapper, ISortHelper<CustomerTask> sortHelper, ICustomerQuestRepository customerQuestRepo, IQuestItemRepository questItemRepo)
    {
        _customerTaskRepo = customerTaskRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
        _customerQuestRepo = customerQuestRepo;
        _questItemRepo = questItemRepo;
    }

    public PageList<CustomerTaskResponseModel> GetAll(CustomerTaskParams @params)
    {
        var listAll = _customerTaskRepo.GetAll();

        //Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<CustomerTaskResponseModel>>(sortedQuests);
        return PageList<CustomerTaskResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }
    public async Task<CustomerTaskResponseModel> Get(int id)
    {
        var entity = await _customerTaskRepo.Get(id);
        CheckDataNotNull("CustomerTask", entity);
        return _mapper.Map<CustomerTaskResponseModel>(entity);
    }

    public async Task<CustomerTaskResponseModel> CreateAsync(CustomerTaskRequestModel request)
    {
        var entity = _mapper.Map<CustomerTask>(request);
        entity = await _customerTaskRepo.Add(entity);
        return _mapper.Map<CustomerTaskResponseModel>(entity);
    }

    public async Task<CustomerTaskResponseModel> UpdateAsync(CustomerTaskRequestModel request)
    {
        var entity = _mapper.Map<CustomerTask>(request);
        entity = await _customerTaskRepo.Update(entity);
        return _mapper.Map<CustomerTaskResponseModel>(entity);
    }

    public async Task<CustomerTaskResponseModel> DeleteAsync(int id)
    {
        var entity = await _customerTaskRepo.Delete(id);
        return _mapper.Map<CustomerTaskResponseModel>(entity);
    }

    public async Task<CustomerTaskResponseModel> UpdateCurrentPointAsync(int id, float currentPoint)
    {
        var customerTask = await _customerTaskRepo.Get(id);
        customerTask.CurrentPoint = currentPoint;
        customerTask = await _customerTaskRepo.UpdateFields(customerTask, r => r.CurrentPoint);
        return _mapper.Map<CustomerTaskResponseModel>(customerTask);
    }

    public async Task<CustomerTaskResponseModel> UpdateStatusAsync(int id, string status)
    {
        var customerTask = await _customerTaskRepo.Get(id);
        customerTask.Status = status;
        customerTask = await _customerTaskRepo.UpdateFields(customerTask, r => r.Status ?? string.Empty);
        return _mapper.Map<CustomerTaskResponseModel>(customerTask);
    }

    public string GetBeginPointsAsync(int customerQuestId)
    {
        return _customerQuestRepo.Get(customerQuestId).Result.BeginPoint!;
    }

    public async Task<CustomerTaskResponseModel> DecreasePointWhenHitSuggestion(int customerQuestId)
    {
        var currentPoint = _customerTaskRepo
            .GetAllByCondition(x => x.CustomerQuestId == customerQuestId)
            .OrderByDescending(x => x.CurrentPoint)
            .LastOrDefaultAsync().Result!.CurrentPoint;

        var customerTask = await _customerTaskRepo.GetAllByCondition(x => x.CustomerQuestId == customerQuestId)
            .OrderByDescending(x => x.CurrentPoint)
            .LastOrDefaultAsync();

        customerTask!.CurrentPoint = currentPoint - PointWhenHitSuggestion;
        customerTask = await _customerTaskRepo.UpdateFields(customerTask, r => r.CurrentPoint);
        return _mapper.Map<CustomerTaskResponseModel>(customerTask);
    }

    public async Task<CustomerTaskResponseModel> DecreasePointWhenWrongAnswer(int customerQuestId)
    {
        var currentPoint = _customerTaskRepo
            .GetAllByCondition(x => x.CustomerQuestId == customerQuestId)
            .OrderByDescending(x => x.CurrentPoint)
            .LastOrDefaultAsync().Result!.CurrentPoint;

        var customerTask = await _customerTaskRepo.GetAllByCondition(x => x.CustomerQuestId == customerQuestId)
            .OrderByDescending(x => x.CurrentPoint)
            .LastOrDefaultAsync();

        customerTask!.CurrentPoint = currentPoint - PointWhenWrongAnswer;
        customerTask = await _customerTaskRepo.UpdateFields(customerTask, r => r.CurrentPoint);
        return _mapper.Map<CustomerTaskResponseModel>(customerTask);
    }

    public Task<bool> IsLastQuestItem(int customerQuestId)
    {
        var numOfItemCustomerHasDone = _customerTaskRepo
            .GetAllByCondition(x => x.CustomerQuestId == customerQuestId).Count();
        var numOfItemInQuest = _questItemRepo
            .GetAllByCondition(x => x.QuestId == _customerQuestRepo.Get(customerQuestId).Result.QuestId).Count();
        return Task.FromResult(numOfItemCustomerHasDone == numOfItemInQuest);
    }

    public float GetLastPoint(int customerQuestId)
    {
        return _customerTaskRepo
            .GetAllByCondition(x => x.CustomerQuestId == customerQuestId)
            .OrderByDescending(x => x.CurrentPoint)
            .LastOrDefaultAsync().Result!.CurrentPoint;
    }
}