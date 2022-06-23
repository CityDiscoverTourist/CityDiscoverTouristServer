using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class UserSubscribedRepository : GenericRepository<UserSubscribed, int>, IUserSubscribedRepository
{
    public UserSubscribedRepository(ApplicationDbContext context) : base(context)
    {
    }
}