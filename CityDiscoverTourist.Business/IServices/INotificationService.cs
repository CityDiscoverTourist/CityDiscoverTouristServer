using CityDiscoverTourist.Business.Data.ResponseModel;

namespace CityDiscoverTourist.Business.IServices;

public interface INotificationService
{
    public Task<string> SendNotification(NotificationRequestModel notificationRequestModel);
}