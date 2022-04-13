using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface ITaskService
{
    public PageList<Entity> GetAll(TaskParams @params);
    public Task<TaskResponseModel> Get(int id, string? fields);
    public Task<TaskResponseModel> CreateAsync(TaskRequestModel request);
    public Task<TaskResponseModel> UpdateAsync(TaskRequestModel request);
    public Task<TaskResponseModel> DeleteAsync(int id);
}