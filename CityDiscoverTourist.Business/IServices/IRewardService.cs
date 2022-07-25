using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface IRewardService
{
    public PageList<RewardResponseModel> GetAll(RewardParams @params);
    public PageList<RewardResponseModel> GetByCustomerId(RewardParams @params, string customerId);
    public Task<RewardResponseModel> Get(int id);
    public Task<RewardResponseModel> CreateAsync(RewardRequestModel request);
    public Task<RewardResponseModel> InvalidReward();
    public Task<RewardResponseModel> DeleteAsync(int id);
}