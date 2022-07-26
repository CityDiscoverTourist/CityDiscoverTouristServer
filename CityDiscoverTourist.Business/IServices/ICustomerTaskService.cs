using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using Microsoft.AspNetCore.Http;

namespace CityDiscoverTourist.Business.IServices;

public interface ICustomerTaskService
{
    public Task<PageList<CustomerTaskResponseModel>> GetAll(CustomerTaskParams @params);
    public Task<PageList<CustomerTaskResponseModel>> GetByCustomerQuestId(int customerQuestId,
        CustomerTaskParams @params);
    public Task<CustomerTaskResponseModel> Get(int id);
    public Task<CustomerTaskResponseModel> CustomerStartQuest(CustomerTaskRequestModel request, int questId);
    public Task<int> MoveCustomerToNextTask(int questId, int customerQuestId);
    public Task<CustomerTaskResponseModel> DeleteAsync(int id);
    public string GetBeginPointsAsync(int customerQuestId);
    public Task<CustomerTaskResponseModel> DecreasePointWhenHitSuggestion(int customerQuestId);
    public Task<CustomerTaskResponseModel> CheckCustomerAnswer(int customerQuestId, string customerReply,
        int questItemId, List<IFormFile>? files = null);
    public Task<bool> IsLastQuestItem(int customerQuestId);
    public float GetLastPoint(int customerQuestId);
    public IEnumerable<string> GetLongLatFromCurrentQuestItemOfCustomer(int customerQuestId);
    public bool IsCustomerAtQuestItemLocation(int customerQuestId, float latitude, float longitude);
    public Task<string> ShowSuggestions(int questItemId);
    public bool CheckCustomerLocationWithQuestLocation(int questId, float latitude, float longitude);
}