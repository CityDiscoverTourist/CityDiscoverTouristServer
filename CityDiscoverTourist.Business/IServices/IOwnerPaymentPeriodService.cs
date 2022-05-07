using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface IOwnerPaymentPeriodService
{
    public PageList<OwnerPaymentPeriod> GetAll(OwnerPaymentPeriodParams @params);
    public Task<OwnerPaymentPeriod> Get(int id);
    public Task<OwnerPaymentPeriod> CreateAsync(OwnerPaymentPeriodRm request);
    public Task<OwnerPaymentPeriod> UpdateAsync(OwnerPaymentPeriodRm request);
    public Task<OwnerPaymentPeriod> DeleteAsync(int id);
}