using CityDiscoverTourist.Data.Models;
using Task = CityDiscoverTourist.Data.Models.Task;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class TaskRepository : GenericRepository<Task, int>, ITaskRepository
{
    public TaskRepository(ApplicationDbContext context) : base(context)
    {
    }
}