using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface ICompetitionService
{
    public PageList<Competition> GetAll(CompetitionParams @params);
    public Task<Competition> Get(int id);
    public Task<Competition> CreateAsync(Competition request);
    public Task<Competition> UpdateAsync(Competition request);
    public Task<Competition> DeleteAsync(int id);
}