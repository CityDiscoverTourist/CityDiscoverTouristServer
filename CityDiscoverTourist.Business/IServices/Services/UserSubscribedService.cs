using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices.Services;

public class UserSubscribedService : IUserSubscribedService
{
    private readonly IUserSubscribedRepository _userSubscribedRepository;

    public UserSubscribedService(IUserSubscribedRepository userSubscribedRepository)
    {
        _userSubscribedRepository = userSubscribedRepository;
    }

    public Task<UserSubscribed> CreateAsync(UserSubscribed request)
    {
        return _userSubscribedRepository.Add(request);
    }
}