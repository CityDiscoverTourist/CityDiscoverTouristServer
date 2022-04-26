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
public class TaskTypeController : ControllerBase
{
    private readonly ITaskTypeService _taskTypeService;

    public TaskTypeController(ITaskTypeService taskTypeService)
    {
        _taskTypeService = taskTypeService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<TaskTypeResponseModel>> GetAll([FromQuery] TaskTypeParams param)
    {
        var entity = _taskTypeService.GetAll(param);

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

        return ApiResponse<List<TaskTypeResponseModel>>.Ok2(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<TaskTypeResponseModel>> Get(int id, string? fields)
    {
        var entity = await _taskTypeService.Get(id, fields);

        return ApiResponse<TaskType>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<TaskTypeResponseModel>> Post(TaskTypeRequestModel data)
    {
        var entity = await _taskTypeService.CreateAsync(data);
        return ApiResponse<TaskType>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<TaskTypeResponseModel>> Put(TaskTypeRequestModel data)
    {
        var entity = await _taskTypeService.UpdateAsync(data);
        return ApiResponse<TaskType>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<TaskTypeResponseModel>>> Delete(int id)
    {
        var entity = await _taskTypeService.DeleteAsync(id);
        return ApiResponse<TaskType>.Ok(entity);
    }

}