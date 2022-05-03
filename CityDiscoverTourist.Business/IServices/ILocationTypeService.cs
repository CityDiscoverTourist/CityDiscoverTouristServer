using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface ILocationTypeService
{
    public PageList<LocationType> GetAll(LocationTypeParams @params);
    public Task<LocationType> Get(int id);
    public Task<LocationType> CreateAsync(LocationTypeRequestModel request);
    public Task<LocationType> UpdateAsync(LocationTypeRequestModel request);
    public Task<LocationType> DeleteAsync(int id);
}