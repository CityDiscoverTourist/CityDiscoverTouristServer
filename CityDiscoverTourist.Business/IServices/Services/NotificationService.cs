using System.Net.Http.Headers;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.HubConfig;
using CityDiscoverTourist.Business.HubConfig.IHub;
using CityDiscoverTourist.Business.Settings;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using CorePush.Google;
using Microsoft.AspNetCore.SignalR;

namespace CityDiscoverTourist.Business.IServices.Services;

public class NotificationService : INotificationService
{
    private readonly NotificationSetting _notificationSettings;
    private readonly INotificationRepository _notificationRepository;
    private readonly IHubContext<NotifyHub, INotifyHub> _hubContext;

    public NotificationService(NotificationSetting notificationSettings, INotificationRepository notificationRepository, IHubContext<NotifyHub, INotifyHub> hubContext)
    {
        _notificationSettings = notificationSettings;
        _notificationRepository = notificationRepository;
        _hubContext = hubContext;
    }

    public async Task<string> SendNotification(NotificationRequestModel notificationRequestModel)
    {
        if (!notificationRequestModel.IsAndroidDevice) return "Failed";
        var settings = new FcmSettings
        {
            SenderId = _notificationSettings.SenderId,
            ServerKey = _notificationSettings.ServerKey
        };

        var httpClient = new HttpClient();

        var authorizationKey = string.Format("key={0}", settings.ServerKey);
        var deviceToken = notificationRequestModel.DeviceId;

        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var dataPayload = new NotificationRequestModel.GoogleNotification.DataPayload
        {
            Title = notificationRequestModel.Title,
            Body = notificationRequestModel.Body
        };

        var notification = new NotificationRequestModel.GoogleNotification
        {
            Data = dataPayload,
            Notification = dataPayload
        };

        var fcm = new FcmSender(settings, httpClient);

        var response = await fcm.SendAsync(deviceToken, notification);

        return response.IsSuccess() ? "Success" : "Failed";
    }

    public async Task<Notification> CreateAsync(Notification notification)
    {
        var entity = await _notificationRepository.Add(notification);
        await _hubContext.Clients.All.GetNotification(entity);
        return entity;
    }

    public IQueryable<Notification> GetAllAsync()
    {
        var entity = _notificationRepository.GetAll().OrderByDescending(x => x.CreatedDate);
        _hubContext.Clients.All.GetNotifications(entity);
        return entity;
    }
}