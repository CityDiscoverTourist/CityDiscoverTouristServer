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

[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class QuestItemController : ControllerBase
{
    private readonly IQuestItemService _taskService;

    public QuestItemController(IQuestItemService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<TaskResponseModel>> GetAll([FromQuery] TaskParams param)
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

        return ApiResponse<List<TaskResponseModel>>.Ok2(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<TaskResponseModel>> Get(int id)
    {
        var entity = await _taskService.Get(id);

        return ApiResponse<QuestItem>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<TaskResponseModel>> Post(TaskRequestModel data)
    {
        var entity = await _taskService.CreateAsync(data);
        return ApiResponse<QuestItem>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<TaskResponseModel>> Put(TaskRequestModel data)
    {
        var entity = await _taskService.UpdateAsync(data);
        return ApiResponse<QuestItem>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<TaskResponseModel>>> Delete(int id)
    {
        var entity = await _taskService.DeleteAsync(id);
        return ApiResponse<QuestItem>.Ok(entity);
    }

    [HttpGet("count-tasks")]
    public int Count([FromQuery] Guid questId)
    {
        return _taskService.CountTaskInQuest(questId);
    }
}