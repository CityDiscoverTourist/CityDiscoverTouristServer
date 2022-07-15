using CityDiscoverTourist.Business.Data.ResponseModel;

namespace CityDiscoverTourist.Business.HubConfig.IHub;

public interface IPaymentHub
{
    Task GetPayment(PaymentResponseModel payment);
}