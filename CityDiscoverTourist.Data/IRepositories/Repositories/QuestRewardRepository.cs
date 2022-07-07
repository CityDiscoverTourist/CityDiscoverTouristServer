using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class QuestRewardRepository : GenericRepository<QuestReward, Guid>, IQuestRewardRepository
{
    public QuestRewardRepository(ApplicationDbContext context) : base(context)
    {
    }
}