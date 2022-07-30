using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface INotificationService
{
    public Task<string> SendNotification(NotificationRequestModel notificationRequestModel);
    public PageList<Notification> GetAllAsync(BaseParam @params, string userId);
    public PageList<Notification> GetAllNotifications(BaseParam @params, string userId);
    public Task<bool> UserHasRead(string userId);
    public Task CreateAsync(NotifyUserRequestModel request);

}