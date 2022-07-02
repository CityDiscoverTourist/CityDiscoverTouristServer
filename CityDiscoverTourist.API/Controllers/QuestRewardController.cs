using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class QuestRewardController : ControllerBase
{
    private readonly IQuestRewardService _questRewardService;

    /// <inheritdoc />
    public QuestRewardController(IQuestRewardService questRewardService)
    {
        _questRewardService = questRewardService;
    }
    /// <summary>
    ///     create area
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<QuestRewardResponseModel>> Post(QuestRewardRequestModel data)
    {
        var entity = await _questRewardService.CreateAsync(data);
        return ApiResponse<Area>.Created(entity);
    }
}