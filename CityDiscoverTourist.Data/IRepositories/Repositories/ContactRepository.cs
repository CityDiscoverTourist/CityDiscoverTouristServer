using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class ContactRepository : GenericRepository<Contact, int>, IContactRepository
{
    public ContactRepository(ApplicationDbContext context) : base(context)
    {
    }
}