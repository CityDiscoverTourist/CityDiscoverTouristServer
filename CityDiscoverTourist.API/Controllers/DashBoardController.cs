using CityDiscoverTourist.Business.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashBoardService;

    public DashboardController(IDashboardService dashBoardService)
    {
        _dashBoardService = dashBoardService;
    }

    [HttpGet]
    public Task<string?[]> GetTopCustomer()
    {
        return Task.FromResult(_dashBoardService.GetTopCustomer());
    }

    [HttpGet("total-revenue")]
    public Task<float> GetRevenue()
    {
        return Task.FromResult(_dashBoardService.GetTotalRevenue());
    }

    [HttpGet("total-user")]
    public Task<int> GetCountUser()
    {
        return Task.FromResult(_dashBoardService.TotalAccount());
    }

    [HttpGet("total-quest")]
    public Task<int> GetTotalQuest()
    {
        return Task.FromResult(_dashBoardService.TotalQuest());
    }

    [HttpGet("top-play")]
    public Task<string[]> GetTopPlayQuest()
    {
        return Task.FromResult(_dashBoardService.GetTopQuests());
    }
}