using CityDiscoverTourist.Business.Data.RequestModel;

namespace CityDiscoverTourist.Business.HubConfig.IHub;

public interface IChatHub
{
    Task AdminSendMessageToCustomer(ChatHubRequestModel request);
    Task CustomerSendMessageToAdmin(ChatHubRequestModel request);
}