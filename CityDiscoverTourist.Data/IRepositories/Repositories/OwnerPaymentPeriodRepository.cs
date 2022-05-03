using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class OwnerPaymentPeriodRepository : GenericRepository<OwnerPaymentPeriod, int>, IOwnerPaymentPeriodRepository
{
    public OwnerPaymentPeriodRepository(ApplicationDbContext context) : base(context)
    {
    }
}