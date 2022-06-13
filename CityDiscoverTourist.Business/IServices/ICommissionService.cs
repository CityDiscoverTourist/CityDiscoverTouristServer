using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface ICommissionService
{
    public PageList<CommissionResponseModel> GetAll(CommissionParams @params);
    public Task<CommissionResponseModel> Get(int id);
    public Task<CommissionResponseModel> CreateAsync(CommissionRequestModel request);
    public Task<CommissionResponseModel> UpdateAsync(CommissionRequestModel request);
    public Task<CommissionResponseModel> DeleteAsync(int id);
}