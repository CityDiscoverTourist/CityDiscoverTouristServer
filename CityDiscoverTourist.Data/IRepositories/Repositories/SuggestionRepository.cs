using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class SuggestionRepository : GenericRepository<Suggestion, int>, ISuggestionRepository
{
    public SuggestionRepository(ApplicationDbContext context) : base(context)
    {
    }
}