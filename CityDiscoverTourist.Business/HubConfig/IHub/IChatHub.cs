namespace CityDiscoverTourist.Business.HubConfig.IHub;

public interface IChatHub
{
    Task AdminSendMessageToCustomer(string user, string message, string conId);
    Task CustomerSendMessageToAdmin(string user, string message);
}