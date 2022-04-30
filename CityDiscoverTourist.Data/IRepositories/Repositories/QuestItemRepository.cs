using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class QuestItemRepository : GenericRepository<QuestItem, int>, IQuestItemRepository
{
    public QuestItemRepository(ApplicationDbContext context) : base(context)
    {
    }
}