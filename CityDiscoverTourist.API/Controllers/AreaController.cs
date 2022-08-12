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
public class AreaController : ControllerBase
{
    private readonly IAreaService _areaService;

    /// <inheritdoc />
    public AreaController(IAreaService areaService)
    {
        _areaService = areaService;
    }

    /// <summary>
    ///     get all areas
    /// </summary>
    /// <param name="param"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    //[Cached(600)]
    public ApiResponse<PageList<AreaResponseModel>> GetAll([FromQuery] AreaParams param, Language language = Language.vi)
    {
        var entity = _areaService.GetAll(param, language);

        var metadata = new
        {
            entity.TotalCount,
            entity.TotalPages,
            entity.PageSize,
            entity.CurrentPage,
            entity.HasNext,
            entity.HasPrevious
        };

        return ApiResponse<List<AreaResponseModel>>.Success(entity, metadata);
    }

    /// <summary>
    ///     get area by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    //[Cached(600)]
    public async Task<ApiResponse<AreaResponseModel>> Get(int id, Language language = Language.vi)
    {
        var entity = await _areaService.Get(id, language);

        return ApiResponse<Area>.Ok(entity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}/not-language")]
    [Authorize(Roles = "Admin")]
    //[Cached(600)]
    public async Task<ApiResponse<AreaResponseModel>> Get(int id)
    {
        var entity = await _areaService.Get(id);

        return ApiResponse<Area>.Ok(entity);
    }
    /// <summary>
    ///     create area
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    //[AllowAnonymous]
    public async Task<ApiResponse<AreaResponseModel>> Post(AreaRequestModel data)
    {
        var entity = await _areaService.CreateAsync(data);
        return ApiResponse<Area>.Created(entity);
    }

    /// <summary>
    ///     update area
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<AreaResponseModel>> Put([FromBody] AreaRequestModel data)
    {
        var entity = await _areaService.UpdateAsync(data);
        return ApiResponse<Area>.Created(entity);
    }

    /// <summary>
    ///     soft  delete area
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<AreaResponseModel>>> Delete(int id)
    {
        var entity = await _areaService.DeleteAsync(id);
        return ApiResponse<Area>.Ok(entity);
    }

    /// <summary>
    ///     disable area
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("disable/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<AreaResponseModel>>> Disable(int id)
    {
        var entity = await _areaService.DisableAsync(id);
        return ApiResponse<Area>.Ok(entity);
    }

    /// <summary>
    ///     enable area
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("enable/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<AreaResponseModel>>> Enable(int id)
    {
        var entity = await _areaService.EnableAsync(id);
        return ApiResponse<Area>.Ok(entity);
    }

    /// <summary>
    ///     check area by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet("check")]
    [Authorize(Roles = "Admin")]
    //[Cached(600)]
    public Task<bool> CheckExist(string name)
    {
        var entity = _areaService.CheckExisted(name);

        return entity;
    }
}