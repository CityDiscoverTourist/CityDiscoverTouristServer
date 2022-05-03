using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface IPaymentService
{
    public PageList<Payment> GetAll(PaymentParams @params);
    public Task<Payment> Get(int id);
    public Task<Payment> CreateAsync(Payment request);
    public Task<Payment> UpdateAsync(Payment request);
    public Task<Payment> DeleteAsync(int id);
}