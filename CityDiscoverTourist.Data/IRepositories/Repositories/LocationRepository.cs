using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class LocationRepository : GenericRepository<Location, int>, ILocationRepository
{
    public LocationRepository(ApplicationDbContext context) : base(context)
    {
    }
}