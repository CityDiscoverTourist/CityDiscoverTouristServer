using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface INotificationService
{
    public Task<string> SendNotification(NotificationRequestModel notificationRequestModel);
    public Task<List<Notification>> GetAllAsync(string userId);
    public Task<bool> UserHasRead(string userId);
    public Task CreateAsync(NotifyUserRequestModel request);

}