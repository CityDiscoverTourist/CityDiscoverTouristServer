using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface IPaymentService
{
    public PageList<PaymentRequestModel> GetAll(PaymentParams @params);
    public Task<PaymentRequestModel> Get(int id);
    public Task<PaymentRequestModel> CreateAsync(PaymentRequestModel request);
    public Task<PaymentRequestModel> UpdateAsync(PaymentRequestModel request);
    public Task<PaymentRequestModel> DeleteAsync(int id);
}