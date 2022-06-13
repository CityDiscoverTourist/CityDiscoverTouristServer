using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface IOwnerPaymentPeriodService
{
    public PageList<OwnerPaymentPeriodResponseModel> GetAll(OwnerPaymentPeriodParams @params);
    public Task<OwnerPaymentPeriodResponseModel> Get(int id);
    public Task<OwnerPaymentPeriodResponseModel> CreateAsync(OwnerPaymentPeriodRm request);
    public Task<OwnerPaymentPeriodResponseModel> UpdateAsync(OwnerPaymentPeriodRm request);
    public Task<OwnerPaymentPeriodResponseModel> DeleteAsync(int id);
}