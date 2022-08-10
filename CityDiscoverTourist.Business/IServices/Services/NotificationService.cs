using System.Net.Http.Headers;
using AutoMapper;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.HubConfig;
using CityDiscoverTourist.Business.HubConfig.IHub;
using CityDiscoverTourist.Business.Settings;
using CityDiscoverTourist.Data.IRepositories;
using CityDiscoverTourist.Data.Models;
using CorePush.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace CityDiscoverTourist.Business.IServices.Services;

public class NotificationService : BaseService, INotificationService
{
    private readonly NotificationSetting _notificationSettings;
    private readonly INotificationRepository _notificationRepository;
    private readonly IHubContext<NotifyHub, INotifyHub> _hubContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly INotifyUserRepository _notifyUserRepository;

    public NotificationService(NotificationSetting notificationSettings, INotificationRepository notificationRepository, IHubContext<NotifyHub, INotifyHub> hubContext, UserManager<ApplicationUser> userManager, IMapper mapper, INotifyUserRepository notifyUserRepository)
    {
        _notificationSettings = notificationSettings;
        _notificationRepository = notificationRepository;
        _hubContext = hubContext;
        _userManager = userManager;
        _mapper = mapper;
        _notifyUserRepository = notifyUserRepository;
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

    public PageList<Notification> GetAllAsync(BaseParam @params, string userId)
    {
        var notifyUser = _notifyUserRepository.GetByCondition(x => x.UserId == userId).Where(x => x.HasRead == false);
        var notify = new List<Notification>();
        foreach (var item in notifyUser)
        {
            var notification = _notificationRepository.GetByCondition(x => x.Id == item.NotifyId).ToList();

            notify.AddRange(notification);
        }

        return PageList<Notification>.ToPageList(notify.OrderByDescending(x => x.CreatedDate), @params.PageNumber, @params.PageSize);
    }

    public PageList<Notification> GetAllNotifications(BaseParam @params, string userId)
    {
        var notifyUser = _notifyUserRepository.GetByCondition(x => x.UserId == userId);
        var notify = new List<Notification>();
        foreach (var item in notifyUser)
        {
            var notification = _notificationRepository.GetByCondition(x => x.Id == item.NotifyId).ToList();

            notify.AddRange(notification);
        }

        return PageList<Notification>.ToPageList(notify.OrderByDescending(x => x.CreatedDate), @params.PageNumber, @params.PageSize);
    }

    public async Task<bool> UserHasRead(string userId)
    {
        var notifyUser = _notifyUserRepository.GetByCondition(x => x.UserId == userId).Where(x => x.HasRead == false);
        foreach (var item in notifyUser)
        {
            item.HasRead = true;
            await _notifyUserRepository.Update(item);
        }
        return true;
    }

    public async Task CreateAsync(NotifyUserRequestModel request)
    {
        var mappedData = _mapper.Map<Notification>(request);

        var notify = await _notificationRepository.Add(mappedData);

        //send notification to client
        await _hubContext.Clients.All.GetNotification(notify);

        var admin = _userManager.Users.ToList();
        foreach (var item in admin)
        {
            if (await _userManager.IsInRoleAsync(item, Role.Admin.ToString()))
            {
                await _notifyUserRepository.Add(new NotifyUser
                {
                    NotifyId = notify.Id,
                    UserId = item.Id,
                    HasRead = false
                });
            }
        }
    }
}