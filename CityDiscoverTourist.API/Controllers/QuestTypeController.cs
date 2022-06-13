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
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class QuestTypeController : ControllerBase
{
    private readonly IQuestTypeService _questTypeService;

    /// <summary>
    /// </summary>
    /// <param name="questTypeService"></param>
    public QuestTypeController(IQuestTypeService questTypeService)
    {
        _questTypeService = questTypeService;
    }

    /// <summary>
    ///     get all quest types
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<QuestTypeResponseModel>> GetAll([FromQuery] QuestTypeParams param)
    {
        var entity = _questTypeService.GetAll(param);

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

        return ApiResponse<List<QuestTypeResponseModel>>.Success(entity, metadata);
    }

    /// <summary>
    ///     get quest type by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<QuestTypeResponseModel>> Get(int id)
    {
        var entity = await _questTypeService.Get(id);

        return ApiResponse<QuestType>.Ok(entity);
    }

    /// <summary>
    ///     count all quest in quest type
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("count")]
    //[Cached(600)]
    public async Task<OkObjectResult> CountQuestInQuestType(int id)
    {
        var entity = await _questTypeService.CountQuestInQuestType(id);

        return Ok(entity);
    }

    /// <summary>
    ///     create quest type
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<QuestTypeResponseModel>> Post([FromForm] QuestTypeRequestModel data)
    {
        var entity = await _questTypeService.CreateAsync(data);
        return ApiResponse<QuestType>.Created(entity);
    }

    /// <summary>
    ///     update quest type
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ApiResponse<QuestTypeResponseModel>> Put([FromForm] QuestTypeRequestModel data)
    {
        var entity = await _questTypeService.UpdateAsync(data);
        return ApiResponse<QuestType>.Created(entity);
    }

    /// <summary>
    ///     delete quest type
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<QuestTypeResponseModel>>> Delete(int id)
    {
        var entity = await _questTypeService.DeleteAsync(id);
        return ApiResponse<QuestType>.Ok(entity);
    }
}