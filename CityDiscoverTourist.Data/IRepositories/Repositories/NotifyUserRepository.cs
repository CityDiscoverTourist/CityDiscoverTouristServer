using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class NotifyUserRepository : GenericRepository<NotifyUser, int>, INotifyUserRepository
{
    public NotifyUserRepository(ApplicationDbContext context) : base(context)
    {
    }
}