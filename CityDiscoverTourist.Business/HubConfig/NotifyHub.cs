using CityDiscoverTourist.Business.HubConfig.IHub;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.SignalR;

namespace CityDiscoverTourist.Business.HubConfig;

public class NotifyHub : Hub<INotifyHub>
{
    public async Task GetNotifications(List<Notification> notification)
    {
        await Clients.All.GetNotifications(notification);
    }

    public async Task GetNotification(Notification notification)
    {
        await Clients.All.GetNotification(notification);
    }
}