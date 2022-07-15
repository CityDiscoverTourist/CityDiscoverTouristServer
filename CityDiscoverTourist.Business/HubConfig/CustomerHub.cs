using CityDiscoverTourist.Business.HubConfig.IHub;
using Microsoft.AspNetCore.SignalR;

namespace CityDiscoverTourist.Business.HubConfig;

public class CustomerHub : Hub<ICustomerHub>
{
    public async Task NewCustomerCreated(string email)
    {
        await Clients.All.NewCustomerCreated(email);
    }
}