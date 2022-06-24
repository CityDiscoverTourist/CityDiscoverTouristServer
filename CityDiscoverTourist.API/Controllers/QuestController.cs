using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class QuestController : ControllerBase
{
    private readonly IQuestService _questService;

    /// <summary>
    /// </summary>
    /// <param name="questService"></param>
    public QuestController(IQuestService questService)
    {
        _questService = questService;
    }

    /// <summary>
    ///     get all quests
    /// </summary>
    /// <param name="param"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<QuestResponseModel>> GetAll([FromQuery] QuestParams param, Language language = Language.vi)
    {
        var entity = _questService.GetAll(param, language);

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

        return ApiResponse<List<QuestResponseModel>>.Success(entity, metadata);
    }

    /// <summary>
    ///     get quest by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<QuestResponseModel>> Get(int id, Language language = Language.vi)
    {
        var entity = await _questService.Get(id, language);

        return ApiResponse<QuestResponseModel>.Ok(entity);
    }

    /// <summary>
    ///     create a new quest
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<QuestResponseModel>> Post([FromForm] QuestRequestModel data)
    {
        var entity = await _questService.CreateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    /// <summary>
    ///     update quest
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ApiResponse<QuestResponseModel>> Put([FromForm] QuestRequestModel data)
    {
        var entity = await _questService.UpdateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    /// <summary>
    ///   soft  delete quest
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<QuestResponseModel>>> Delete(int id)
    {
        var entity = await _questService.DeleteAsync(id);
        return ApiResponse<Quest>.Ok(entity);
    }

    /// <summary>
    /// disable quest
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("disable/{id:int}")]
    public async Task<ActionResult<ApiResponse<QuestResponseModel>>> Disable(int id)
    {
        var entity = await _questService.DisableAsync(id);
        return ApiResponse<Quest>.Ok(entity);
    }

    /// <summary>
    /// Enable quest
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("enable/{id:int}")]
    public async Task<ActionResult<ApiResponse<QuestResponseModel>>> Enable(int id)
    {
        var entity = await _questService.EnableAsync(id);
        return ApiResponse<Quest>.Ok(entity);
    }

    /// <summary>
    /// update status of included entity (enable/disable)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPut("update-status-fk/{id:int}")]
    public Task<ActionResult<ApiResponse<QuestResponseModel>>> UpdateStatusFkKey(int id, string status)
    {
        var entity = _questService.UpdateStatusForeignKey(id, status);
        return Task.FromResult<ActionResult<ApiResponse<QuestResponseModel>>>(ApiResponse<Quest>.Ok(entity.Result));
    }
}