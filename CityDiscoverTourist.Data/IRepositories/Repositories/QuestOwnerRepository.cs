using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class QuestOwnerRepository : GenericRepository<QuestOwner, int>, IQuestOwnerRepository
{
    public QuestOwnerRepository(ApplicationDbContext context) : base(context)
    {
    }
}