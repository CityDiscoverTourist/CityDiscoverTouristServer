using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Business.IServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CityDiscoverTourist.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class HubController : ControllerBase
{
    private IHubContext<AaHub, IHubService> _hubContext;

    public HubController(IHubContext<AaHub, IHubService> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpGet]
    public string Get()
    {
        var a = Guid.NewGuid().ToString();
        try
        {
            var msg = _hubContext.Clients.All.GetMessage(a);
            return msg.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    public string Post(string message)
    {
        var retMessage = string.Empty;
        try
        {
            var msg = _hubContext.Clients.All.GetMessage(message);
            retMessage = "ok";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return retMessage;
    }
}