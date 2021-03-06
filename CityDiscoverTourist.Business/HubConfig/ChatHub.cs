using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.HubConfig.IHub;
using Microsoft.AspNetCore.SignalR;

namespace CityDiscoverTourist.Business.HubConfig;

public class ChatHub : Hub<IChatHub>
{
    public string GetConnectionId() => Context.ConnectionId;

    public async Task AdminSendMessageToCustomer(ChatHubRequestModel requestModel)
    {
        await Clients.Client(requestModel.ConId).AdminSendMessageToCustomer(requestModel);
    }

    public async Task CustomerSendMessageToAdmin(ChatHubRequestModel request)
    {
        await Clients.All.CustomerSendMessageToAdmin(request);
    }
}