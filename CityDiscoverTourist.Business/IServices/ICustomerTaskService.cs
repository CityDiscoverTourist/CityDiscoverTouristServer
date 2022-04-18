using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface ICustomerTaskService
{
    public Task<CustomerTaskResponseModel> Get(int id);
    public Task<CustomerTaskResponseModel> CreateAsync(CustomerTaskRequestModel request);
    public Task<CustomerTaskResponseModel> UpdateAsync(CustomerTaskRequestModel request);
    public Task<CustomerTaskResponseModel> DeleteAsync(int id);
}