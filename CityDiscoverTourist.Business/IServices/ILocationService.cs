using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface ILocationService
{
    public PageList<Location> GetAll(LocationParams @params);
    public Task<Location> Get(int id);
    public Task<Location> CreateAsync(LocationRequestModel request);
    public Task<Location> UpdateAsync(LocationRequestModel request);
    public Task<Location> DeleteAsync(int id);
}