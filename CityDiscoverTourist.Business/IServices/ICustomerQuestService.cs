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
    public Task<CustomerQuestResponseModel> GiveFeedback(int id, CommentRequestModel comment);
    public Task<CustomerQuestResponseModel> InvalidCustomerQuest();
    public Task<List<CustomerQuestResponseModel>> GetByCustomerId(string id, Language language = Language.vi);
    public Task<CustomerQuestResponseModel> CreateAsync(CustomerQuestRequestModel request);
    public Task<CustomerQuestResponseModel> DeleteAsync(int id);
    public Task<CustomerQuestResponseModel> UpdateEndPointAndStatusWhenFinishQuestAsync(int customerQuestId);
    public Task<PageList<CommentResponseModel>> ShowComments(int questId, CustomerQuestParams param);
    public Task<List<CommentResponseModel>> UpdateComment(int customerQuestId, CommentRequestModel comment);
    public IQueryable<CommentResponseModel> GetMyComment( int questId, string customerId);
    public Task ApproveFeedback(int id);
    public Task ForceDelete(int id, bool forceDelete);
}