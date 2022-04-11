using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface IQuestTypeService
{
    public PageList<Entity> GetAll(QuestTypeParams @params);
    public Task<QuestTypeResponseModel> Get(int id, string? fields);
    public Task<QuestTypeResponseModel> CreateAsync(QuestTypeRequestModel request);
    public Task<QuestTypeResponseModel> UpdateAsync(QuestTypeRequestModel request);
    public Task<QuestTypeResponseModel> DeleteAsync(int id);
}