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
public class LocationTypeController : ControllerBase
{
    private readonly ILocationTypeService _locationTypeService;

    /// <summary>
    /// </summary>
    /// <param name="locationTypeService"></param>
    public LocationTypeController(ILocationTypeService locationTypeService)
    {
        _locationTypeService = locationTypeService;
    }

    /// <summary>
    ///     get all location types
    /// </summary>
    /// <param name="param"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<LocationTypeResponseModel>> GetAll([FromQuery] LocationTypeParams param,
        Language language = Language.vi)
    {
        var entity = _locationTypeService.GetAll(param, language);

        var metadata = new
        {
            entity.TotalCount,
            entity.TotalPages,
            entity.PageSize,
            entity.CurrentPage,
            entity.HasNext,
            entity.HasPrevious
        };

        return ApiResponse<List<LocationType>>.Success(entity, metadata);
    }

    /// <summary>
    ///     get location type by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<LocationTypeResponseModel>> Get(int id, Language language = Language.vi)
    {
        var entity = await _locationTypeService.Get(id, language);

        return ApiResponse<LocationType>.Ok(entity);
    }

    /// <summary>
    ///     create location type
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<LocationTypeResponseModel>> Post(LocationTypeRequestModel data)
    {
        var entity = await _locationTypeService.CreateAsync(data);
        return ApiResponse<LocationType>.Created(entity);
    }

    /// <summary>
    ///     update location type
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<LocationTypeResponseModel>> Put([FromBody] LocationTypeRequestModel data)
    {
        var entity = await _locationTypeService.UpdateAsync(data);
        return ApiResponse<LocationType>.Created(entity);
    }

    /// <summary>
    ///     soft  delete location type
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<LocationTypeResponseModel>>> Delete(int id)
    {
        var entity = await _locationTypeService.DeleteAsync(id);
        return ApiResponse<LocationType>.Ok(entity);
    }

    /// <summary>
    ///     disable location type
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("disable/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<LocationTypeResponseModel>>> Disable(int id)
    {
        var entity = await _locationTypeService.DisableAsync(id);
        return ApiResponse<LocationType>.Ok(entity);
    }

    /// <summary>
    ///     enable location type
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("enable/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<LocationTypeResponseModel>>> Enable(int id)
    {
        var entity = await _locationTypeService.EnableAsync(id);
        return ApiResponse<LocationType>.Ok(entity);
    }

    /// <summary>
    ///     check locationtype by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet("check")]
    //[Cached(600)]
    public Task<bool> CheckExist(string name)
    {
        var entity = _locationTypeService.CheckExisted(name);

        return entity;
    }
}