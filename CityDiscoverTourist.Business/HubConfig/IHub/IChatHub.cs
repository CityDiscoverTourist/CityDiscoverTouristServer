
using CityDiscoverTourist.Business.Data.RequestModel;

namespace CityDiscoverTourist.Business.HubConfig.IHub;

public interface IChatHub
{
    Task AdminSendMessageToCustomer(ChatHubRequestModel request);
    Task CustomerSendMessageToAdmin(ChatHubRequestModel request);
    Task AppendMessage(ChatHubRequestModel request);
    Task GetConnectedClients(string contextConnectionId);
}