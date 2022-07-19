using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;

namespace CityDiscoverTourist.Business.IServices;

public interface IPaymentService
{
    public PageList<PaymentResponseModel> GetAll(PaymentParams @params, Language language);
    public Task<PaymentResponseModel> Get(Guid id, Language language);
    public Task<PaymentResponseModel> UpdateStatusWhenSuccess(MomoRequestModel request);
    public Task<List<PaymentResponseModel>> GetByCustomerId(string customerId);
    public Task<string[]> CreateAsync(PaymentRequestModel request, Guid discountCode = default);
    public Task<string[]> PaymentMobile(PaymentRequestModel request, Guid discountCode = default);
    public Task<PaymentResponseModel> InvalidOrder();
    public Task<PaymentResponseModel> DeleteAsync(Guid id);
    public int GetQuantityOfPayment(Guid id);
}