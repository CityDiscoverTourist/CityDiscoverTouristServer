using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface ICityService
{
    public PageList<City> GetAll(CityParams @params);
    public Task<City> Get(int id);
    public Task<City> CreateAsync(CityRequestModel request);
    public Task<City> UpdateAsync(CityRequestModel request);
    public Task<City> DeleteAsync(int id);
}