using System.Net.Http.Headers;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Settings;
using CorePush.Google;

namespace CityDiscoverTourist.Business.IServices.Services;

public class NotificationService : INotificationService
{
    private readonly NotificationSetting _notificationSettings;

    public NotificationService(NotificationSetting notificationSettings)
    {
        _notificationSettings = notificationSettings;
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
}