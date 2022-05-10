using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface ICompetitionService
{
    public PageList<CompetitionResponseModel> GetAll(CompetitionParams @params);
    public Task<CompetitionResponseModel> Get(int id);
    public Task<CompetitionResponseModel> CreateAsync(CompetitionRequestModel request);
    public Task<CompetitionResponseModel> UpdateAsync(CompetitionRequestModel request);
    public Task<CompetitionResponseModel> DeleteAsync(int id);
}