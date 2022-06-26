using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface IPaymentService
{
    public PageList<PaymentResponseModel> GetAll(PaymentParams @params);
    public Task<PaymentRequestModel> Get(int id);
    public Task<string> CreateAsync(PaymentRequestModel request);
    public Task<PaymentResponseModel> UpdateAsync(int id, PaymentRequestModel request);
    public Task<PaymentResponseModel> UpdateAsync(PaymentRequestModel request);
    public Task<PaymentResponseModel> DeleteAsync(int id);
}