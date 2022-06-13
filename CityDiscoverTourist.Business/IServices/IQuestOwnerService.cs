using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface IQuestOwnerService
{
    public PageList<QuestOwnerResponseModel> GetAll(QuestOwnerParams @params);
    public Task<QuestOwnerResponseModel> Get(int id);
    public Task<QuestOwnerResponseModel> CreateAsync(QuestOwnerRequestModel request);
    public Task<QuestOwnerResponseModel> UpdateAsync(QuestOwnerRequestModel request);
    public Task<QuestOwnerResponseModel> DeleteAsync(int id);
}