using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface INotificationService
{
    public Task<string> SendNotification(NotificationRequestModel notificationRequestModel);
    public Task<Notification> CreateAsync(Notification notification);
    public Task<List<Notification>> GetAllAsync();
}