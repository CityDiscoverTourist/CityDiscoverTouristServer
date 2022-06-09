using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
///
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class QuestItemController : ControllerBase
{
    private readonly IQuestItemService _taskService;

    /// <summary>
    ///
    /// </summary>
    /// <param name="taskService"></param>
    public QuestItemController(IQuestItemService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    /// get all quest items
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<QuestItemResponseModel>> GetAll([FromQuery] TaskParams param)
    {
        var entity = _taskService.GetAll(param);

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

        return ApiResponse<List<QuestItemResponseModel>>.Success(entity, metadata);
    }

    /// <summary>
    /// get quest item by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<QuestItemResponseModel>> Get(int id)
    {
        var entity = await _taskService.Get(id);

        return ApiResponse<QuestItem>.Ok(entity);
    }


    /// <summary>
    /// get quest item by quest id
    /// </summary>
    /// <param name="questId"></param>
    /// <returns></returns>
    [HttpGet("get-by-quest-id/{questId:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<QuestItemResponseModel>> GetByQuestId(int questId)
    {
        var entity = await _taskService.GetByQuestId(questId);

        return ApiResponse<QuestItem>.Ok(entity);
    }

    /// <summary>
    /// create quest item
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<QuestItemResponseModel>> Post(QuestItemRequestModel data)
    {
        var entity = await _taskService.CreateAsync(data);
        return ApiResponse<QuestItem>.Created(entity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ApiResponse<QuestItemResponseModel>> Put(QuestItemRequestModel data)
    {
        var entity = await _taskService.UpdateAsync(data);
        return ApiResponse<QuestItem>.Created(entity);
    }

    /// <summary>
    /// delete quest item
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<QuestItemResponseModel>>> Delete(int id)
    {
        var entity = await _taskService.DeleteAsync(id);
        return ApiResponse<QuestItem>.Ok(entity);
    }
}