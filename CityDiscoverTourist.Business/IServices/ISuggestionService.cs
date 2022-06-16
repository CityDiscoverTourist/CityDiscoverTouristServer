using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface ISuggestionService
{
    public PageList<SuggestionResponseModel> GetAll(SuggestionParams @params);
    public Task<SuggestionResponseModel> Get(int id);
    public Task<SuggestionResponseModel> CreateAsync(SuggestionRequestModel request);
    public Task<SuggestionResponseModel> UpdateAsync(SuggestionRequestModel request);
    public Task<SuggestionResponseModel> DeleteAsync(int id);
    public Task<SuggestionResponseModel> DisableAsync(int id);
    public Task<SuggestionResponseModel> EnableAsync(int id);
}