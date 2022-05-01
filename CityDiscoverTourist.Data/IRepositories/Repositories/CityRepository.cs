using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class CityRepository : GenericRepository<City, int>, ICityRepository
{
    public CityRepository(ApplicationDbContext context) : base(context)
    {
    }
}