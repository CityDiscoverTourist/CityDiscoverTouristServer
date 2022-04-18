using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface ICustomerAnswerService
{
    public Task<CustomerAnswer> Get(int id);
    public Task<CustomerAnswer> CreateAsync(CustomerAnswer request);
    public Task<CustomerAnswer> UpdateAsync(CustomerAnswer request);
    public Task<CustomerAnswer> DeleteAsync(int id);
}