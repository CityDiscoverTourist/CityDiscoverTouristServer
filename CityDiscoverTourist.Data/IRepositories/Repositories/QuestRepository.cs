using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class QuestRepository: GenericRepository<Quest, int>, IQuestRepository
{
    public QuestRepository(ApplicationDbContext context) : base(context)
    {
    }
}