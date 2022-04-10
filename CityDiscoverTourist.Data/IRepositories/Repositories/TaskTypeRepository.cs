using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Data.IRepositories.Repositories;

public class TaskTypeRepository : GenericRepository<TaskType, int>, ITaskTypeRepository
{
    public TaskTypeRepository(ApplicationDbContext context) : base(context)
    {
    }
}