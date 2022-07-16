using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.HubConfig.IHub;

public interface INotifyHub
{
    Task GetNotifications(IQueryable<Notification> notification);
    Task GetNotification(Notification notification);
}