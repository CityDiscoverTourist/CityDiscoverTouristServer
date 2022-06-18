using CityDiscoverTourist.Business.DataManager;
using CityDiscoverTourist.Business.HubConfig;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Business.IServices.Services;
using CityDiscoverTourist.Business.Timmer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CityDiscoverTourist.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class ChartController : ControllerBase
{
    private IHubContext<ChartHub> _hubContext;


    public ChartController(IHubContext<ChartHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var timmerManager = new TimmerManager(() => _hubContext.Clients.All.SendAsync("transferchartdata", DataManager.GetData()));
        return Ok(new { Message = "Request Completed" });
    }
}