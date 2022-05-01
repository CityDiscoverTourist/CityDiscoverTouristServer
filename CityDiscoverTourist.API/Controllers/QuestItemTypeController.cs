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
public class QuestItemTypeController : ControllerBase
{
    private readonly IQuestItemTypeService _taskTypeService;

    public QuestItemTypeController(IQuestItemTypeService taskTypeService)
    {
        _taskTypeService = taskTypeService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<QuestItemTypeResponseModel>> GetAll([FromQuery] TaskTypeParams param)
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

        return ApiResponse<List<QuestItemTypeResponseModel>>.Ok2(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<QuestItemTypeResponseModel>> Get(int id, string? fields)
    {
        var entity = await _taskTypeService.Get(id, fields);

        return ApiResponse<QuestItemType>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<QuestItemTypeResponseModel>> Post(QuestItemTypeRequestModel data)
    {
        var entity = await _taskTypeService.CreateAsync(data);
        return ApiResponse<QuestItemType>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<QuestItemTypeResponseModel>> Put(QuestItemTypeRequestModel data)
    {
        var entity = await _taskTypeService.UpdateAsync(data);
        return ApiResponse<QuestItemType>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<QuestItemTypeResponseModel>>> Delete(int id)
    {
        var entity = await _taskTypeService.DeleteAsync(id);
        return ApiResponse<QuestItemType>.Ok(entity);
    }

}