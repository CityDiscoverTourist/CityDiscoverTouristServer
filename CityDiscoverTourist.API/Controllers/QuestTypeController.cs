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
public class QuestTypeController : ControllerBase
{
    private readonly IQuestTypeService _questTypeService;

    public QuestTypeController(IQuestTypeService questTypeService)
    {
        _questTypeService = questTypeService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<Entity>> GetTutorRequest([FromQuery] QuestTypeParams param)
    {
        var entity = _questTypeService.GetAll(param);

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

        return ApiResponse<List<Entity>>.Ok2(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<QuestTypeResponseModel>> Get(int id, string? fields)
    {
        var entity = await _questTypeService.Get(id, fields);

        return ApiResponse<QuestType>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<QuestTypeResponseModel>> Post(QuestTypeRequestModel data)
    {
        var entity = await _questTypeService.CreateAsync(data);
        return ApiResponse<QuestType>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<QuestTypeResponseModel>> Put(QuestTypeRequestModel data)
    {
        var entity = await _questTypeService.UpdateAsync(data);
        return ApiResponse<QuestType>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<QuestTypeResponseModel>>> Delete(int id)
    {
        var entity = await _questTypeService.DeleteAsync(id);
        return ApiResponse<QuestType>.Ok(entity);
    }

}