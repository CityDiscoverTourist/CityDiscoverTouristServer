using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface IQuestItemTypeService
{
    public PageList<QuestItemTypeResponseModel> GetAll(TaskTypeParams @params);

    public Task<QuestItemTypeResponseModel> Get(int id, string? fields);
    public Task<QuestItemTypeResponseModel> CreateAsync(QuestItemTypeRequestModel request);
    public Task<QuestItemTypeResponseModel> UpdateAsync(QuestItemTypeRequestModel request);
    public Task<QuestItemTypeResponseModel> DeleteAsync(int id);
    public Task<QuestItemTypeResponseModel> DisableAsync(int id);
    public Task<QuestItemTypeResponseModel> EnableAsync(int id);
    public Task<QuestItemTypeResponseModel> UpdateStatusForeignKey(int id, string status);
}