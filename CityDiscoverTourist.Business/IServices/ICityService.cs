using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface ICityService
{
    public PageList<CityResponseModel> GetAll(CityParams @params);
    public Task<CityResponseModel> Get(int id);
    public Task<CityResponseModel> CreateAsync(CityRequestModel request);
    public Task<CityResponseModel> UpdateAsync(CityRequestModel request);
    public Task<CityResponseModel> DeleteAsync(int id);
    public Task<CityResponseModel> DisableAsync(int id);
    public Task<CityResponseModel> EnableAsync(int id);
    public Task<CityResponseModel> UpdateStatusForeignKey(int cityId, string status);
    public Task<bool> CheckExisted(string name);
}