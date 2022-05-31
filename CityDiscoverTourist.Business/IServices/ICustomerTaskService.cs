using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface ICustomerTaskService
{
    public PageList<CustomerTaskResponseModel> GetAll(CustomerTaskParams @params);
    public Task<CustomerTaskResponseModel> Get(int id);
    public Task<CustomerTaskResponseModel> CreateAsync(CustomerTaskRequestModel request);
    public Task<CustomerTaskResponseModel> UpdateAsync(CustomerTaskRequestModel request);
    public Task<CustomerTaskResponseModel> DeleteAsync(int id);
    public Task<CustomerTaskResponseModel> UpdateCurrentPointAsync(int id, float currentPoint);
    public Task<CustomerTaskResponseModel> UpdateStatusAsync(int id, string status);
    public string GetBeginPointsAsync(int customerQuestId);
    public Task<CustomerTaskResponseModel> DecreasePointWhenHitSuggestion(int customerQuestId);
    public Task<CustomerTaskResponseModel> DecreasePointWhenWrongAnswer(int customerQuestId);
    public Task<bool> IsLastQuestItem(int customerQuestId);
    public float GetLastPoint(int customerQuestId);
    public IEnumerable<string> GetLongLatFromCurrentQuestItemOfCustomer(int customerQuestId);
    public float DistanceBetweenCustomerLocationAndQuestItem(int customerQuestId, float longitude, float latitude);
}