using CityDiscoverTourist.Business.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
///
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
}