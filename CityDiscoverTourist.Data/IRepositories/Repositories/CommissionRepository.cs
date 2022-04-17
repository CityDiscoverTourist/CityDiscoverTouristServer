using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class CommissionRepository : GenericRepository<Commission, int>, ICommissionRepository
{
    public CommissionRepository(ApplicationDbContext context) : base(context)
    {
    }
}