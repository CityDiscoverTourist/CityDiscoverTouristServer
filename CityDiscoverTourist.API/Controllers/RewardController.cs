using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

        return ApiResponse<List<RewardResponseModel>>.Success(entity, metadata);
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
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
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<OkObjectResult> Put()
    {
        _recurringJobManager.AddOrUpdate("Reward", () => _rewardService.InvalidReward(), Cron.Daily(23, 55));
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