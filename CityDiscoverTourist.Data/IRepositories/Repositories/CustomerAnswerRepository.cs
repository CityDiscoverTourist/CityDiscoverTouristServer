using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class CustomerAnswerRepository : GenericRepository<CustomerAnswer, int>, ICustomerAnswerRepository
{
    public CustomerAnswerRepository(ApplicationDbContext context) : base(context)
    {
    }
}