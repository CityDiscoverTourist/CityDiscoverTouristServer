using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface IQuestItemService
{
    public PageList<QuestItemResponseModel> GetAll(TaskParams @params);
    public Task<QuestItemResponseModel> Get(int id);
    public Task<QuestItemResponseModel> CreateAsync(QuestItemRequestModel request);
    public Task<QuestItemResponseModel> UpdateAsync(QuestItemRequestModel request);
    public Task<QuestItemResponseModel> DeleteAsync(int id);
    //public int CountTaskInQuest(Guid questId);
}