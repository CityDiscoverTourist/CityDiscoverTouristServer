using CityDiscoverTourist.Business.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CityDiscoverTourist.Business.IServices.Services.DashboardService;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
[Authorize(Roles = "Admin, QuestOwner")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashBoardService;

    /// <summary>
    /// </summary>
    /// <param name="dashBoardService"></param>
    public DashboardController(IDashboardService dashBoardService)
    {
        _dashBoardService = dashBoardService;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<Player[]> GetTopCustomer()
    {
        return Task.FromResult(_dashBoardService.GetTopCustomer());
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    [HttpGet("total-revenue")]
    public Task<float> GetRevenue()
    {
        return Task.FromResult(_dashBoardService.GetTotalRevenue());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="month"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    [HttpGet("revenue-by-month")]
    public Task<float> GetRevenueByMonthOfYear(int month, int year)
    {
        return Task.FromResult(_dashBoardService.GetRevenueByMonthOfYear(month, year));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    [HttpGet("revenue-all-month")]
    public Task<float[]> GetRevenueAllMonthOfYear(int year)
    {
        return Task.FromResult(_dashBoardService.GetRevenueByAllMonth(year));
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    [HttpGet("total-user")]
    public Task<int> GetCountUser()
    {
        return Task.FromResult(_dashBoardService.TotalAccount());
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    [HttpGet("total-quest")]
    public Task<int> GetTotalQuest()
    {
        return Task.FromResult(_dashBoardService.TotalQuest());
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    [HttpGet("top-play")]
    public Task<QuestDashboard[]> GetTopPlayQuest(int year = 2022)
    {
        return Task.FromResult(_dashBoardService.GetTopQuests(year));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="month"></param>
    /// <param name="year"></param>
    /// <returns></returns>
    [HttpGet("top-quest-by-month")]
    public Task<QuestDashboard[]> GetTopPlayQuestByMoth(int month, int year = 2022)
    {
        return Task.FromResult(_dashBoardService.GetTopQuestByMonth(month, year));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    [HttpGet("top-quest-by-year")]
    public Task<QuestDashboard[]> GetTopPlayQuestByYear(int year = 2022)
    {
        return _dashBoardService.GetTopQuestByMonthInYear(year);
    }
}