using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface IOwnerPaymentService
{
    public PageList<OwnerPaymentResponseModel> GetAll(OwnerPaymentParams @params);
    public Task<OwnerPaymentResponseModel> Get(int id);
    public Task<OwnerPaymentResponseModel> CreateAsync(OwnerPaymentRequestModel request);
    public Task<OwnerPaymentResponseModel> UpdateAsync(OwnerPaymentRequestModel request);
    public Task<OwnerPaymentResponseModel> DeleteAsync(int id);
}