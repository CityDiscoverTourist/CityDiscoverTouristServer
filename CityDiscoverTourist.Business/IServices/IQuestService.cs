using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface IQuestService
{
    public PageList<QuestResponseModel> GetAll( QuestParams param, Language language);
    public Task<QuestResponseModel> Get(int id, Language language);
    public Task<QuestResponseModel> CreateAsync(QuestRequestModel request);
    public Task<QuestResponseModel> UpdateAsync(QuestRequestModel request);
    public Task<QuestResponseModel> DeleteAsync(int questId);
    public Task<QuestResponseModel> DisableAsync(int questId);
    public Task<QuestResponseModel> EnableAsync(int questId);
    public Task<QuestResponseModel> UpdateStatusForeignKey(int questId, string status);
    public Task<QuestResponseModel> Get(int id);
}