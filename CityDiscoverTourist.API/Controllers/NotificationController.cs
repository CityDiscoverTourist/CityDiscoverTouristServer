using CityDiscoverTourist.API.Response;
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
    ///
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IQueryable<Notification> GetAllNotifications()
    {
        return _notificationService.GetAllAsync();
    }
}