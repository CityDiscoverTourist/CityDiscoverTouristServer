using CityDiscoverTourist.Business.Data.RequestModel;
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
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("to-admin")]
    public IActionResult PostToAdmin(ChatHubRequestModel request)
    {
        _hubContext.Clients.All.CustomerSendMessageToAdmin(request);
        return Ok();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("admin-to-admin")]
    public IActionResult AdminToAdmin(ChatHubRequestModel request)
    {
        _hubContext.Clients.All.AppendMessage(request);
        return Ok();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("to-customer")]
    public IActionResult Post(ChatHubRequestModel request)
    {
        _hubContext.Clients.All.AdminSendMessageToCustomer(request);
        return Ok();
    }
}