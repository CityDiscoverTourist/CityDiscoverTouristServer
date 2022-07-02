using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;

namespace CityDiscoverTourist.Business.IServices;

public interface IQuestRewardService
{
    //public PageList<QuestRewardResponseModel> GetAll(AreaParams @params);
    public Task<QuestRewardResponseModel> CreateAsync(QuestRewardRequestModel request);
}