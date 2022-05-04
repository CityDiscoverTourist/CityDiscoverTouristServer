using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface ICustomerAnswerService
{
    public PageList<CustomerAnswer> GetAll(CustomerAnswerParams @params);
    public Task<CustomerAnswer> Get(int id);
    public Task<CustomerAnswer> CreateAsync(CustomerAnswerRequetModel request);
    public Task<CustomerAnswer> UpdateAsync(CustomerAnswerRequetModel request);
    public Task<CustomerAnswer> DeleteAsync(int id);
}