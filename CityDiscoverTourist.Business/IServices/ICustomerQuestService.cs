using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface ICustomerQuestService
{
    public PageList<CustomerQuestResponseModel> GetAll(CustomerQuestParams @params);
    public Task<CustomerQuestResponseModel> Get(int id);
    public Task<List<CustomerQuestResponseModel>> GetByCustomerId(string id);
    public Task<CustomerQuestResponseModel> CreateAsync(CustomerQuestRequestModel request);
    public Task<CustomerQuestResponseModel> DeleteAsync(int id);
    public Task<CustomerQuestResponseModel> UpdateEndPointAndStatusWhenFinishQuestAsync(int customerQuestId);
    public Task<List<CommentResponseModel>> ShowComments(int questId);
}