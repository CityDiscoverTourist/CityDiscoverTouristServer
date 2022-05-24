using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CustomerTaskService: BaseService, ICustomerTaskService
{
    private readonly ICustomerTaskRepository _customerTaskService;
    private readonly IMapper _mapper;
    private readonly ISortHelper<CustomerTask> _sortHelper;


    public CustomerTaskService(ICustomerTaskRepository noteRepository, IMapper mapper, ISortHelper<CustomerTask> sortHelper)
    {
        _customerTaskService = noteRepository;
        _mapper = mapper;
        _sortHelper = sortHelper;
    }

    public PageList<CustomerTaskResponseModel> GetAll(CustomerTaskParams @params)
    {
        var listAll = _customerTaskService.GetAll();

        //Search(ref listAll, @params);

        var sortedQuests = _sortHelper.ApplySort(listAll, @params.OrderBy);
        var mappedData = _mapper.Map<IEnumerable<CustomerTaskResponseModel>>(sortedQuests);
        return PageList<CustomerTaskResponseModel>.ToPageList(mappedData, @params.PageNumber, @params.PageSize);
    }
    public async Task<CustomerTaskResponseModel> Get(int id)
    {
        var entity = await _customerTaskService.Get(id);
        CheckDataNotNull("CustomerTask", entity);
        return _mapper.Map<CustomerTaskResponseModel>(entity);
    }

    public async Task<CustomerTaskResponseModel> CreateAsync(CustomerTaskRequestModel request)
    {
        var entity = _mapper.Map<CustomerTask>(request);
        entity = await _customerTaskService.Add(entity);
        return _mapper.Map<CustomerTaskResponseModel>(entity);
    }

    public async Task<CustomerTaskResponseModel> UpdateAsync(CustomerTaskRequestModel request)
    {
        var entity = _mapper.Map<CustomerTask>(request);
        entity = await _customerTaskService.Update(entity);
        return _mapper.Map<CustomerTaskResponseModel>(entity);
    }

    public async Task<CustomerTaskResponseModel> DeleteAsync(int id)
    {
        var entity = await _customerTaskService.Delete(id);
        return _mapper.Map<CustomerTaskResponseModel>(entity);
    }

    public async Task<CustomerTaskResponseModel> UpdateCurrentPointAsync(int id, float currentPoint)
    {
        var customerTask = await _customerTaskService.Get(id);
        customerTask.CurrentPoint = currentPoint;
        customerTask = await _customerTaskService.UpdateFields(customerTask, r => r.CurrentPoint);
        return _mapper.Map<CustomerTaskResponseModel>(customerTask);
    }

    public async Task<CustomerTaskResponseModel> UpdateStatusAsync(int id, string status)
    {
        var customerTask = await _customerTaskService.Get(id);
        customerTask.Status = status;
        customerTask = await _customerTaskService.UpdateFields(customerTask, r => r.Status ?? string.Empty);
        return _mapper.Map<CustomerTaskResponseModel>(customerTask);
    }
}