using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface ISuggestionService
{
    public PageList<Suggestion> GetAll(SuggestionParams @params);
    public Task<Suggestion> Get(int id);
    public Task<Suggestion> CreateAsync(SuggestionRequestModel request);
    public Task<Suggestion> UpdateAsync(SuggestionRequestModel request);
    public Task<Suggestion> DeleteAsync(int id);
}