using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface ICustomerService
{
    public PageList<CustomerResponseModel> GetAll(CustomerParams @params);
    public Task<CustomerResponseModel> Get(string id);
    public Task<CustomerResponseModel> CreateAsync(ApplicationUser request);
    public Task<CustomerResponseModel> UpdateAsync(ApplicationUser request);
    public Task<CustomerResponseModel> DeleteAsync(int id);
}