using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class LocationTypeRepository : GenericRepository<LocationType, int>, ILocationTypeRepository
{
    public LocationTypeRepository(ApplicationDbContext context) : base(context)
    {
    }
}