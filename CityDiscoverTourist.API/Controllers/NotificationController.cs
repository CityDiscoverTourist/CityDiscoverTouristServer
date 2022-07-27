using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    /// <summary>
    /// </summary>
    /// <param name="notificationService"></param>
    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    ///  this endpoint use for push notify by mobile device
    /// </summary>
    /// <param name="notificationModel"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SendNotification(NotificationRequestModel notificationModel)
    {
        var result = await _notificationService.SendNotification(notificationModel);
        return Ok(result);
    }

    /// <summary>
    ///     push this to hub
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ApiResponse<List<Notification>>> GetAllNotifications(string userId)
    {
        var entity = await _notificationService.GetAllAsync(userId);
        return ApiResponse<Notification>.Ok(entity);
    }

    /// <summary>
    ///     use this to update the notification status of admin is read or not
    /// </summary>
    /// <param name="userId"></param>
    [HttpGet("update-notify-status/{userId}")]
    public async Task UserHasRead(string userId)
    {
        await _notificationService.UserHasRead(userId);
    }
}