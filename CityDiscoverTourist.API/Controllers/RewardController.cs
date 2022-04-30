using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CityDiscoverTourist.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class RewardController : ControllerBase
{
    private readonly IRewardService _rewardService;

    public RewardController(IRewardService taskService)
    {
        _rewardService = taskService;
    }

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
            entity.HasPrevious,
        };
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

        return ApiResponse<List<RewardResponseModel>>.Ok2(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<RewardResponseModel>> Get(int id)
    {
        var entity = await _rewardService.Get(id);

        return ApiResponse<Task>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<RewardResponseModel>> Post(RewardRequestModel data)
    {
        var entity = await _rewardService.CreateAsync(data);
        return ApiResponse<Task>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<RewardResponseModel>> Put(RewardRequestModel data)
    {
        var entity = await _rewardService.UpdateAsync(data);
        return ApiResponse<Task>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<RewardResponseModel>>> Delete(int id)
    {
        var entity = await _rewardService.DeleteAsync(id);
        return ApiResponse<Task>.Ok(entity);
    }

}