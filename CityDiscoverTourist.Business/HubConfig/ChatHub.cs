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
        /*//await Clients.Client(requestModel.AdminId).AdminSendMessageToCustomer(requestModel);
        await Clients.Clients(requestModel.AdminId).AppendMessage(requestModel);*/
    }

    public async Task AppendMessage(ChatHubRequestModel requestModel)
    {
        //await Clients.Client(requestModel.ConId).AppendMessage(requestModel);
        await Clients.AllExcept(requestModel.AdminId).AppendMessage(requestModel);
    }

    public async Task CustomerSendMessageToAdmin(ChatHubRequestModel request)
    {
        await Clients.All.CustomerSendMessageToAdmin(request);
    }
}