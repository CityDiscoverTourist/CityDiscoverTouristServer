using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.HubConfig.IHub;
using Microsoft.AspNetCore.SignalR;

namespace CityDiscoverTourist.Business.HubConfig;

public class PaymentHub : Hub<IPaymentHub>
{
    public async Task GetPayment(PaymentResponseModel payment)
    {
        await Clients.All.GetPayment(payment);
    }
}