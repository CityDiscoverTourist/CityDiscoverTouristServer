using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class CustomerTaskRepository : GenericRepository<CustomerTask, int>, ICustomerTaskRepository
{
    public CustomerTaskRepository(ApplicationDbContext context) : base(context)
    {
    }
}