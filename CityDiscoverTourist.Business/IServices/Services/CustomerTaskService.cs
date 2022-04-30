using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class CustomerTaskService: ICustomerTaskService
{
    private readonly ICustomerTaskRepository _customerTaskService;
    private readonly IMapper _mapper;

    public CustomerTaskService(ICustomerTaskRepository noteRepository, IMapper mapper)
    {
        _customerTaskService = noteRepository;
        _mapper = mapper;
    }

    public async Task<CustomerTaskResponseModel> Get(int id)
    {
        var entity = await _customerTaskService.Get(id);

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
}