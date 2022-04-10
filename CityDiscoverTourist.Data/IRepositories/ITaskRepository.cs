using Task = CityDiscoverTourist.Data.Models.Task;

namespace CityDiscoverTourist.Data.IRepositories;

public interface ITaskRepository: IGenericRepository<Task, int>
{

}