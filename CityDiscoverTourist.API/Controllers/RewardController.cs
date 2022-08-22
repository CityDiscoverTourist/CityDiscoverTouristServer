using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
[Authorize]
public class RewardController : ControllerBase
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IRewardService _rewardService;

    /// <summary>
    /// </summary>
    /// <param name="taskService"></param>
    /// <param name="recurringJobManager"></param>
    public RewardController(IRewardService taskService, IRecurringJobManager recurringJobManager)
    {
        _rewardService = taskService;
        _recurringJobManager = recurringJobManager;
    }

    /// <summary>
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    //[Cached(600)]
    public ApiResponse<PageList<RewardResponseModel>> GetAll([FromQuery] RewardParams param)
    {
        var entity = _rewardService.GetAll(param);

        var metadata = new
        {
            entity.TotalCount,
            entity.TotalPages,
            entity.PageSize,
            entity.CurrentPage,
            entity.HasNext,
            entity.HasPrevious
        };

        return ApiResponse<List<RewardResponseModel>>.Success(entity, metadata);
    }

    /// <summary>
    /// </summary>
    /// <param name="param"></param>
    /// <param name="customerId"></param>
    /// <returns></returns>
    [HttpGet("get-by-customer-id")]
    //[AllowAnonymous]
    //[Cached(600)]
    public ApiResponse<PageList<RewardResponseModel>> GetByCustomerId([FromQuery] RewardParams param, string customerId)
    {
        var entity = _rewardService.GetByCustomerId(param, customerId);

        var metadata = new
        {
            entity.TotalCount,
            entity.TotalPages,
            entity.PageSize,
            entity.CurrentPage,
            entity.HasNext,
            entity.HasPrevious
        };

        return ApiResponse<List<RewardResponseModel>>.Success(entity, metadata);
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    //[Cached(600)]
    public async Task<ApiResponse<RewardResponseModel>> Get(int id)
    {
        var entity = await _rewardService.Get(id);

        return ApiResponse<Task>.Ok(entity);
    }

    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<RewardResponseModel>> Post(RewardRequestModel data)
    {
        var entity = await _rewardService.CreateAsync(data);
        return ApiResponse<Task>.Created(entity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [HttpPost("notification")]
    [Authorize(Roles = "Admin")]
    public Task Notification()
    {
        _recurringJobManager.AddOrUpdate("Notification Reward", () => _rewardService.PushNotification(), "0 0 * * *",
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        return Task.CompletedTask;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    public async Task<OkObjectResult> Put()
    {
        _recurringJobManager.AddOrUpdate("Reward", () => _rewardService.InvalidReward(), "0 0 * * *",
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        return await Task.FromResult(Ok("RecurringJobManager"));
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<RewardResponseModel>>> Delete(int id)
    {
        var entity = await _rewardService.DeleteAsync(id);
        return ApiResponse<Task>.Ok(entity);
    }
}