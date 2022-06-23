using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface IUserSubscribedService
{
    public Task<UserSubscribed> CreateAsync(UserSubscribed request);
}