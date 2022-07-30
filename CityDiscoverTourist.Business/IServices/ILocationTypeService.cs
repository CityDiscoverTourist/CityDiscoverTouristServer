using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface ILocationTypeService
{
    public PageList<LocationTypeResponseModel> GetAll(LocationTypeParams @params, Language language = Language.vi);
    public Task<LocationTypeResponseModel> Get(int id, Language language = Language.vi);
    public Task<LocationTypeResponseModel> Get(int id);
    public Task<LocationTypeResponseModel> CreateAsync(LocationTypeRequestModel request);
    public Task<LocationTypeResponseModel> UpdateAsync(LocationTypeRequestModel request);
    public Task<LocationTypeResponseModel> DeleteAsync(int id);
    public Task<LocationTypeResponseModel> DisableAsync(int id);
    public Task<LocationTypeResponseModel> EnableAsync(int id);
    public Task<bool> CheckExisted(string name);
}