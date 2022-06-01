using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface IQuestTypeService
{
    public PageList<QuestTypeResponseModel> GetAll(QuestTypeParams @params);
    public Task<QuestTypeResponseModel> Get(int id);
    public Task<QuestTypeResponseModel> CreateAsync(QuestTypeRequestModel request);
    public Task<QuestTypeResponseModel> UpdateAsync(QuestTypeRequestModel request);
    public Task<QuestTypeResponseModel> DeleteAsync(int id);
    public Task<int> CountQuestInQuestType(int questTypeId);
}