using CityDiscoverTourist.Business.HubConfig;
using CityDiscoverTourist.Business.HubConfig.IHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
///
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class SupportChannelController : ControllerBase
{
    private readonly IHubContext<ChatHub, IChatHub> _hubContext;

    /// <summary>
    ///
    /// </summary>
    /// <param name="hubContext"></param>
    public SupportChannelController(IHubContext<ChatHub, IChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost("to-admin")]
    public IActionResult Post(string message, string user)
    {
        _hubContext.Clients.All.CustomerSendMessageToAdmin(user, message);
        return Ok();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="user"></param>
    /// <param name="conId"></param>
    /// <returns></returns>
    [HttpPost("to-customer")]
    public IActionResult Post(string message, string user, string conId)
    {
        _hubContext.Clients.All.AdminSendMessageToCustomer(user, message, conId);
        return Ok();
    }
}