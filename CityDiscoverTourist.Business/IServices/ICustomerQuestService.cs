using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface ICustomerQuestService
{
    public Task<CustomerQuestResponseModel> Get(int id);
    public Task<CustomerQuestResponseModel> CreateAsync(CustomerQuestRequestModel request);
    public Task<CustomerQuestResponseModel> UpdateAsync(CustomerQuestRequestModel request);
    public Task<CustomerQuestResponseModel> DeleteAsync(int id);
}