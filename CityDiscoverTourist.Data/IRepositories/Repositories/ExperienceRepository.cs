using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class ExperienceRepository : GenericRepository<Experience, int>, IExperienceRepository
{
    public ExperienceRepository(ApplicationDbContext context) : base(context)
    {
    }
}