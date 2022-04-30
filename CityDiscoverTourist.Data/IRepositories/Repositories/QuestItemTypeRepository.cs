using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class QuestItemTypeRepository : GenericRepository<QuestItemType, int>, IQuestItemTypeRepository
{
    public QuestItemTypeRepository(ApplicationDbContext context) : base(context)
    {
    }
}