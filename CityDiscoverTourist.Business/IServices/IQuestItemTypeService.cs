using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface IQuestItemTypeService
{
    public PageList<TaskTypeResponseModel> GetAll(TaskTypeParams @params);

    public Task<TaskTypeResponseModel> Get(int id, string? fields);
    public Task<TaskTypeResponseModel> CreateAsync(TaskTypeRequestModel request);
    public Task<TaskTypeResponseModel> UpdateAsync(TaskTypeRequestModel request);
    public Task<TaskTypeResponseModel> DeleteAsync(int id);
}