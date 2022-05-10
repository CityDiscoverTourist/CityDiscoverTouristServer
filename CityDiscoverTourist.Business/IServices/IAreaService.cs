using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface IAreaService
{
    public PageList<AreaResponseModel> GetAll(AreaParams @params);
    public Task<AreaResponseModel> Get(int id);
    public Task<AreaResponseModel> CreateAsync(AreaRequestModel request);
    public Task<AreaResponseModel> UpdateAsync(AreaRequestModel request);
    public Task<AreaResponseModel> DeleteAsync(int id);
}