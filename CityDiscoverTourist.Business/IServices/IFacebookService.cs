using CityDiscoverTourist.Business.Data.ResponseModel;

namespace CityDiscoverTourist.Business.IServices;

public interface IFacebookService
{
    public Task<FacebookResponseModel> GetUserFromFacebookAsync(string facebookToken);
    public Task<T> GetAsync<T>(string accessToken, string endpoint, string? args = null);
    public Task<LoginResponseModel> LoginFacebookAsync(string token);
}