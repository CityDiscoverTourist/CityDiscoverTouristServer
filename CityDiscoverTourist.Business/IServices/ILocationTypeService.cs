using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface ILocationTypeService
{
    public PageList<LocationTypeResponseModel> GetAll(LocationTypeParams @params);
    public Task<LocationTypeResponseModel> Get(int id);
    public Task<LocationTypeResponseModel> CreateAsync(LocationTypeRequestModel request);
    public Task<LocationTypeResponseModel> UpdateAsync(LocationTypeRequestModel request);
    public Task<LocationTypeResponseModel> DeleteAsync(int id);
}