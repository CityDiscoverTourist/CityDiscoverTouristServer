using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface IPaymentService
{
    public PageList<PaymentResponseModel> GetAll(PaymentParams @params);
    public Task<PaymentResponseModel> Get(Guid id);
    public Task<List<PaymentResponseModel>> GetByCustomerId(string customerId);
    public Task<string> CreateAsync(PaymentRequestModel request);
    public Task<PaymentResponseModel> UpdateAsync(int id, PaymentRequestModel request);
    public Task<PaymentResponseModel> UpdateAsync(PaymentRequestModel request);
    public Task<PaymentResponseModel> DeleteAsync(Guid id);
    public int GetQuantityOfPayment(Guid id);
}