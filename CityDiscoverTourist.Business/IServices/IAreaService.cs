using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface IAreaService
{
    public PageList<AreaResponseModel> GetAll(AreaParams @params, Language language = Language.vi);
    public Task<AreaResponseModel> Get(int id, Language language = Language.vi);
    public Task<AreaResponseModel> Get(int id);
    public Task<AreaResponseModel> CreateAsync(AreaRequestModel request);
    public Task<AreaResponseModel> UpdateAsync(AreaRequestModel request);
    public Task<AreaResponseModel> DeleteAsync(int id);
    public Task<AreaResponseModel> DisableAsync(int id);
    public Task<AreaResponseModel> EnableAsync(int id);
    public Task<bool> CheckExisted(string name);
}