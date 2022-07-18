using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using Microsoft.AspNetCore.Http;

namespace CityDiscoverTourist.Business.IServices;

public interface IQuestItemService
{
    public PageList<QuestItemResponseModel> GetAll(TaskParams @params, Language language);
    public Task<QuestItemResponseModel> Get(int id, Language language);
    public Task<QuestItemResponseModel> CreateAsync(QuestItemRequestModel request);
    public Task<QuestItemResponseModel> UpdateAsync(QuestItemRequestModel request);
    public Task<QuestItemResponseModel> DeleteAsync(int id);
    public Task<QuestItemResponseModel> DisableAsync(int id);
    public Task<QuestItemResponseModel> EnableAsync(int id);
    public IEnumerable<QuestItemResponseModel> GetByQuestId( int id, Language language);
    public Task<QuestItemResponseModel> Get(int id);
}