using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class CompetitionRepository : GenericRepository<Competition, int>, ICompetitionRepository
{
    public CompetitionRepository(ApplicationDbContext context) : base(context)
    {
    }
}