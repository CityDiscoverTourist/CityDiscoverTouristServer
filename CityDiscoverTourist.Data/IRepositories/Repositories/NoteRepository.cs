using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class NoteRepository : GenericRepository<Note, int>, INoteRepository
{
    public NoteRepository(ApplicationDbContext context) : base(context)
    {
    }
}