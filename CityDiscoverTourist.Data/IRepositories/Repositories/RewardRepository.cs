using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class RewardRepository : GenericRepository<Reward, int>, IRewardRepository
{
    public RewardRepository(ApplicationDbContext context) : base(context)
    {
    }
}