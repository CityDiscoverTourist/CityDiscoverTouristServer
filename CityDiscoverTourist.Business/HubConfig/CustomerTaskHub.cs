using CityDiscoverTourist.Business.Data.ResponseModel;
using Microsoft.AspNetCore.SignalR;

namespace CityDiscoverTourist.Business.HubConfig;

public class CustomerTaskHub : Hub
{
    public async Task demo(string message)
    {
        await Clients.All.SendAsync("demo", message);
    }
}