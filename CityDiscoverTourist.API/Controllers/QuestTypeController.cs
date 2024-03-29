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
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    //[Cached(600)]
    public ApiResponse<PageList<QuestTypeResponseModel>> GetAll([FromQuery] QuestTypeParams param,
        Language language = Language.vi)
    {
        var entity = _questTypeService.GetAll(param, language);

        var metadata = new
        {
            entity.TotalCount,
            entity.TotalPages,
            entity.PageSize,
            entity.CurrentPage,
            entity.HasNext,
            entity.HasPrevious
        };

        return ApiResponse<List<QuestTypeResponseModel>>.Success(entity, metadata);
    }

    /// <summary>
    ///     get quest type by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    //[Cached(600)]
    public async Task<ApiResponse<QuestTypeResponseModel>> Get(int id, Language language = Language.vi)
    {
        var entity = await _questTypeService.Get(id, language);

        return ApiResponse<QuestType>.Ok(entity);
    }

    /// <summary>
    ///     get quest type by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}/not-language")]
    [Authorize(Roles = "Admin")]
    //[Cached(600)]
    public async Task<ApiResponse<QuestTypeResponseModel>> Get(int id)
    {
        var entity = await _questTypeService.Get(id);

        return ApiResponse<QuestTypeResponseModel>.Ok(entity);
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
    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<QuestTypeResponseModel>> Put([FromForm] QuestTypeRequestModel data)
    {
        var entity = await _questTypeService.UpdateAsync(data);
        return ApiResponse<QuestType>.Created(entity);
    }

    /// <summary>
    ///     soft  delete quest type
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<QuestTypeResponseModel>>> Delete(int id)
    {
        var entity = await _questTypeService.DeleteAsync(id);
        return ApiResponse<QuestType>.Ok(entity);
    }

    /// <summary>
    ///     disable quest type
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("disable/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<QuestTypeResponseModel>>> Disable(int id)
    {
        var entity = await _questTypeService.DisableAsync(id);
        return ApiResponse<QuestType>.Ok(entity);
    }

    /// <summary>
    ///     Enable quest type
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("enable/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<QuestTypeResponseModel>>> Enable(int id)
    {
        var entity = await _questTypeService.EnableAsync(id);
        return ApiResponse<QuestType>.Ok(entity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet("check")]
    //[Cached(600)]
    public async Task<bool> CheckExist(string name)
    {
        var entity = await _questTypeService.CheckExisted(name);

        return entity;
    }

    /// <summary>
    ///     update status of included entity (enable/disable)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPut("update-status-fk/{id:int}")]
    public Task<ActionResult<ApiResponse<QuestTypeResponseModel>>> UpdateStatusFkKey(int id, string status)
    {
        var entity = _questTypeService.UpdateStatusForeignKey(id, status);
        return Task.FromResult<ActionResult<ApiResponse<QuestTypeResponseModel>>>(
            ApiResponse<QuestType>.Ok(entity.Result));
    }
}