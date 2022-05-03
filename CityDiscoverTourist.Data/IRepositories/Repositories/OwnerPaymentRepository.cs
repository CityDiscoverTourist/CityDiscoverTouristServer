using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class OwnerPaymentRepository : GenericRepository<OwnerPayment, int>, IOwnerPaymentRepository
{
    public OwnerPaymentRepository(ApplicationDbContext context) : base(context)
    {
    }
}