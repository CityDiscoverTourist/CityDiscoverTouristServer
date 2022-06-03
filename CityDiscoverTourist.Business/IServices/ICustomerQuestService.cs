using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface ICustomerQuestService
{
    public PageList<CustomerQuestResponseModel> GetAll(CustomerQuestParams @params);
    public Task<CustomerQuestResponseModel> Get(int id);
    public Task<CustomerQuestResponseModel> CreateAsync(CustomerQuestRequestModel request);
    public Task<CustomerQuestResponseModel> UpdateAsync(CustomerQuestRequestModel request);
    public Task<CustomerQuestResponseModel> DeleteAsync(int id);
    public Task<CustomerQuestResponseModel> UpdateEndPointAndStatusWhenFinishQuestAsync(int id, CommonStatus status);
}