using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
[Authorize]
public class QuestItemController : ControllerBase
{
    private readonly IQuestItemService _taskService;

    /// <summary>
    /// </summary>
    /// <param name="taskService"></param>
    public QuestItemController(IQuestItemService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    ///     get all quest items
    /// </summary>
    /// <param name="param"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    //[Cached(600)]
    public async Task<ApiResponse<PageList<QuestItemResponseModel>>> GetAll([FromQuery] TaskParams param,
        Language language = Language.vi)
    {
        var entity = await _taskService.GetAll(param, language);

        var metadata = new
        {
            entity.TotalCount,
            entity.TotalPages,
            entity.PageSize,
            entity.CurrentPage,
            entity.HasNext,
            entity.HasPrevious
        };

        return ApiResponse<List<QuestItemResponseModel>>.Success(entity, metadata);
    }

    /// <summary>
    ///     get quest item by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    //[Cached(600)]
    public async Task<ApiResponse<QuestItemResponseModel>> Get(int id, Language language = Language.vi)
    {
        var entity = await _taskService.Get(id, language);

        return ApiResponse<QuestItem>.Ok(entity);
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("images/{id:int}")]
    [AllowAnonymous]
    public async Task<List<string>> GetImages(int id)
    {
        var entity = await _taskService.GetListImage(id);

        return entity;
    }

    /// <summary>
    ///     get quest item by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}/not-language")]
    //[Authorize(Roles = "Admin")]
    [AllowAnonymous]
    //[Cached(600)]
    public async Task<ApiResponse<QuestItemResponseModel>> Get(int id)
    {
        var entity = await _taskService.Get(id);

        return ApiResponse<QuestItem>.Ok(entity);
    }


    /// <summary>
    ///     get quest item by quest id
    /// </summary>
    /// <param name="questId"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet("get-by-quest-id/{questId:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<List<QuestItemResponseModel>>> GetByQuestId(int questId,
        Language language = Language.vi)
    {
        var entity = await _taskService.GetByQuestId(questId, language);

        return ApiResponse<QuestItemResponseModel>.Ok(entity);
    }

    /// <summary>
    ///     create quest item
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<QuestItemResponseModel>> Post([FromForm] QuestItemRequestModel data)
    {
        var entity = await _taskService.CreateAsync(data);
        return ApiResponse<QuestItem>.Created(entity);
    }

    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<QuestItemResponseModel>> Put([FromForm] QuestItemRequestModel data)
    {
        var entity = await _taskService.UpdateAsync(data);
        return ApiResponse<QuestItem>.Created(entity);
    }

    /// <summary>
    ///     soft  delete quest item
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<QuestItemResponseModel>>> Delete(int id)
    {
        var entity = await _taskService.DeleteAsync(id);
        return ApiResponse<QuestItem>.Ok(entity);
    }

    /// <summary>
    ///     disable quest item
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("disable/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<QuestItemResponseModel>>> Disable(int id)
    {
        var entity = await _taskService.DisableAsync(id);
        return ApiResponse<QuestItem>.Ok(entity);
    }

    /// <summary>
    ///     Enable quest item
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("enable/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<QuestItemResponseModel>>> Enable(int id)
    {
        var entity = await _taskService.EnableAsync(id);
        return ApiResponse<QuestItem>.Ok(entity);
    }
}