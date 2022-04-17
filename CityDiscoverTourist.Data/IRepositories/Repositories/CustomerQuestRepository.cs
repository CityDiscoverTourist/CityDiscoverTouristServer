using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class CustomerQuestRepository : GenericRepository<CustomerQuest, int>, ICustomerQuestRepository
{
    public CustomerQuestRepository(ApplicationDbContext context) : base(context)
    {
    }
}