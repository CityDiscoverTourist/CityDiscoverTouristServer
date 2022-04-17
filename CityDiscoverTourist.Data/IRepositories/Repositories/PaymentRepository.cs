using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class PaymentRepository : GenericRepository<Payment, int>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext context) : base(context)
    {
    }
}