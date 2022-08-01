using CityDiscoverTourist.Business.HubConfig.IHub;
using Microsoft.AspNetCore.SignalR;

namespace CityDiscoverTourist.Business.HubConfig;

public class ChatHub : Hub<IChatHub>
{
    public string GetConnectionId() => Context.ConnectionId;

    public async Task AdminSendMessageToCustomer(string user, string message, string conId)
    {
        await Clients.Client(conId).AdminSendMessageToCustomer(user, message, conId);
    }

    public async Task CustomerSendMessageToAdmin(string user, string message, string myConId)
    {
        await Clients.All.CustomerSendMessageToAdmin(user, message, myConId);
    }
}