using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface IAreaService
{
    public PageList<Area> GetAll(AreaParams @params);
    public Task<Area> Get(int id);
    public Task<Area> CreateAsync(Area request);
    public Task<Area> UpdateAsync(Area request);
    public Task<Area> DeleteAsync(int id);
}