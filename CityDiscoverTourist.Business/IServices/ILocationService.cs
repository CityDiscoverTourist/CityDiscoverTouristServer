using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface ILocationService
{
    public PageList<LocationResponseModel> GetAll(LocationParams @params);
    public Task<LocationResponseModel> Get(int id);
    public Task<LocationResponseModel> CreateAsync(LocationRequestModel request);
    public Task<LocationResponseModel> UpdateAsync(LocationRequestModel request);
    public Task<LocationResponseModel> UpdateAddressAsync(LocationRequestModel request);
    public Task<LocationResponseModel> DeleteAsync(int id);
    public Task<LocationResponseModel> DisableAsync(int id);
    public Task<LocationResponseModel> EnableAsync(int id);
    public string[] GetLatLongAndPlaceIdFromAddress(string address);
    public Task<bool> CheckExisted(string name);
}