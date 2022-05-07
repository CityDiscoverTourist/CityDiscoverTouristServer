using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface ICommissionService
{
    public PageList<Commission> GetAll(CommissionParams @params);
    public Task<Commission> Get(int id);
    public Task<Commission> CreateAsync(CommissionRequestModel request);
    public Task<Commission> UpdateAsync(CommissionRequestModel request);
    public Task<Commission> DeleteAsync(int id);
}