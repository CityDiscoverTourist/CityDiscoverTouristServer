using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Authorization;
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
    ///     this endpoint use for push notify by mobile device
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
    //[Authorize(Roles = "Admin")]
    public ApiResponse<PageList<Notification>> GetAllNotifications([FromQuery] BaseParam @params, string userId)
    {
        var entity = _notificationService.GetAllAsync(@params, userId);

        var metadata = new
        {
            entity.TotalCount,
            entity.TotalPages,
            entity.PageSize,
            entity.CurrentPage,
            entity.HasNext,
            entity.HasPrevious
        };

        return ApiResponse<List<Notification>>.Success(entity, metadata);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="params"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    [HttpGet("get-all")]
    //[Authorize(Roles = "Admin")]
    public ApiResponse<PageList<Notification>> GetAll([FromQuery] BaseParam @params, string userId)
    {
        var entity = _notificationService.GetAllNotifications(@params, userId);

        var metadata = new
        {
            entity.TotalCount,
            entity.TotalPages,
            entity.PageSize,
            entity.CurrentPage,
            entity.HasNext,
            entity.HasPrevious
        };

        return ApiResponse<List<Notification>>.Success(entity, metadata);
    }

    /// <summary>
    ///     use this to update the notification status of admin is read or not
    /// </summary>
    /// <param name="userId"></param>
    [HttpGet("update-notify-status/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task UserHasRead(string userId)
    {
        await _notificationService.UserHasRead(userId);
    }
}