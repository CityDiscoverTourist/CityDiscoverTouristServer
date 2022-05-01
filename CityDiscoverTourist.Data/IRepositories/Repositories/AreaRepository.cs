using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class AreaRepository : GenericRepository<Area, int>, IAreaRepository
{
    public AreaRepository(ApplicationDbContext context) : base(context)
    {
    }
}