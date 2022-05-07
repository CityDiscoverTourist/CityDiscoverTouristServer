using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface IOwnerPaymentService
{
    public PageList<OwnerPayment> GetAll(OwnerPaymentParams @params);
    public Task<OwnerPayment> Get(int id);
    public Task<OwnerPayment> CreateAsync(OwnerPaymentRequestModel request);
    public Task<OwnerPayment> UpdateAsync(OwnerPaymentRequestModel request);
    public Task<OwnerPayment> DeleteAsync(int id);
}