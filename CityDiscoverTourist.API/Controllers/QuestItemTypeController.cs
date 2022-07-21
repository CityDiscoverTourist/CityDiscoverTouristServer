using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
[Authorize]
public class QuestItemTypeController : ControllerBase
{
    private readonly IQuestItemTypeService _questItemTypeService;

    /// <summary>
    /// </summary>
    /// <param name="questItemTypeService"></param>
    public QuestItemTypeController(IQuestItemTypeService questItemTypeService)
    {
        _questItemTypeService = questItemTypeService;
    }

    /// <summary>
    ///     get all quest item types
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<QuestItemTypeResponseModel>> GetAll([FromQuery] TaskTypeParams param)
    {
        var entity = _questItemTypeService.GetAll(param);

        var metadata = new
        {
            entity.TotalCount,
            entity.TotalPages,
            entity.PageSize,
            entity.CurrentPage,
            entity.HasNext,
            entity.HasPrevious
        };

        return ApiResponse<List<QuestItemTypeResponseModel>>.Success(entity, metadata);
    }

    /// <summary>
    ///     get quest item type by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="fields"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<QuestItemTypeResponseModel>> Get(int id, string? fields)
    {
        var entity = await _questItemTypeService.Get(id, fields);

        return ApiResponse<QuestItemType>.Ok(entity);
    }

    /// <summary>
    ///     create quest item type
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<QuestItemTypeResponseModel>> Post(QuestItemTypeRequestModel data)
    {
        var entity = await _questItemTypeService.CreateAsync(data);
        return ApiResponse<QuestItemType>.Created(entity);
    }

    /// <summary>
    ///     update quest item type
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ApiResponse<QuestItemTypeResponseModel>> Put(QuestItemTypeRequestModel data)
    {
        var entity = await _questItemTypeService.UpdateAsync(data);
        return ApiResponse<QuestItemType>.Created(entity);
    }

    /// <summary>
    ///     soft  delete quest item type
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<QuestItemTypeResponseModel>>> Delete(int id)
    {
        var entity = await _questItemTypeService.DeleteAsync(id);
        return ApiResponse<QuestItemType>.Ok(entity);
    }

    /// <summary>
    ///     disable quest item type
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("disable/{id:int}")]
    public async Task<ActionResult<ApiResponse<QuestItemTypeResponseModel>>> Disable(int id)
    {
        var entity = await _questItemTypeService.DisableAsync(id);
        return ApiResponse<QuestItemType>.Ok(entity);
    }

    /// <summary>
    ///     Enable quest item type
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("enable/{id:int}")]
    public async Task<ActionResult<ApiResponse<QuestItemTypeResponseModel>>> Enable(int id)
    {
        var entity = await _questItemTypeService.EnableAsync(id);
        return ApiResponse<QuestItemType>.Ok(entity);
    }

    /// <summary>
    ///     update status of included entity (enable/disable)
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPut("update-status-fk/{id:int}")]
    public Task<ActionResult<ApiResponse<QuestItemTypeResponseModel>>> UpdateStatusFkKey(int id, string status)
    {
        var entity = _questItemTypeService.UpdateStatusForeignKey(id, status);
        return Task.FromResult<ActionResult<ApiResponse<QuestItemTypeResponseModel>>>(
            ApiResponse<QuestItemType>.Ok(entity.Result));
    }
}