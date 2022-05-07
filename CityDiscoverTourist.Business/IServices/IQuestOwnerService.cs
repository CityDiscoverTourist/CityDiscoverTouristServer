using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface IQuestOwnerService
{
    public PageList<QuestOwner> GetAll(QuestOwnerParams @params);
    public Task<QuestOwner> Get(int id);
    public Task<QuestOwner> CreateAsync(QuestOwnerRequestModel request);
    public Task<QuestOwner> UpdateAsync(QuestOwnerRequestModel request);
    public Task<QuestOwner> DeleteAsync(int id);
}