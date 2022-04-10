using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class QuestTypeRepository : GenericRepository<QuestType, int>, IQuestTypeRepository
{
    public QuestTypeRepository(ApplicationDbContext context) : base(context)
    {
    }
}