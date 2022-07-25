using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface ICustomerService
{
    public PageList<CustomerResponseModel> GetAll(CustomerParams @params);
    public Task<CustomerResponseModel> Get(string id);
    public Task<CustomerResponseModel> UpdateUser(string id, bool isLock);
    public Task<CustomerResponseModel> UpdatePassword( UpdatePasswordModel data);
    public Task<CustomerResponseModel> UpdateAsync(CustomerRequestModel request);
}