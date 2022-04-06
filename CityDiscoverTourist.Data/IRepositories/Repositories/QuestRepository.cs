using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class QuestRepository: GenericRepository<Quest, Guid>, IQuestRepository
{
    public QuestRepository(ApplicationDbContext context) : base(context)
    {
    }
}