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
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    /// <summary>
    /// </summary>
    /// <param name="locationService"></param>
    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    /// <summary>
    ///     get all locations
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<LocationResponseModel>> GetAll([FromQuery] LocationParams param)
    {
        var entity = _locationService.GetAll(param);

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

        return ApiResponse<List<Location>>.Success(entity, metadata);
    }

    /// <summary>
    ///     get location by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<LocationResponseModel>> Get(int id)
    {
        var entity = await _locationService.Get(id);

        return ApiResponse<Location>.Ok(entity);
    }

    /// <summary>
    ///     create location
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<LocationResponseModel>> Post(LocationRequestModel data)
    {
        var entity = await _locationService.CreateAsync(data);
        return ApiResponse<Location>.Created(entity);
    }

    /// <summary>
    ///     update location
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ApiResponse<LocationResponseModel>> Put([FromBody] LocationRequestModel data)
    {
        var entity = await _locationService.UpdateAsync(data);
        return ApiResponse<Location>.Created(entity);
    }

    /// <summary>
    ///     update address in location
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("update-address")]
    public async Task<ApiResponse<LocationResponseModel>> UpdateAddressAsync(LocationRequestModel request)
    {
        var entity = await _locationService.UpdateAddressAsync(request);
        return ApiResponse<Location>.Created(entity);
    }

    /// <summary>
    ///  soft   delete location by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<LocationResponseModel>>> Delete(int id)
    {
        var entity = await _locationService.DeleteAsync(id);
        return ApiResponse<Location>.Ok(entity);
    }

    /// <summary>
    /// disable location by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("disable/{id:int}")]
    public async Task<ActionResult<ApiResponse<LocationResponseModel>>> Disable(int id)
    {
        var entity = await _locationService.DisableAsync(id);
        return ApiResponse<Location>.Ok(entity);
    }

    /// <summary>
    /// enable location by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("enable/{id:int}")]
    public async Task<ActionResult<ApiResponse<LocationResponseModel>>> Enable(int id)
    {
        var entity = await _locationService.EnableAsync(id);
        return ApiResponse<Location>.Ok(entity);
    }

    /// <summary>
    /// check location by name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet("check")]
    //[Cached(600)]
    public Task<bool> CheckExist(string name)
    {
        var entity = _locationService.CheckExisted(name);

        return entity;
    }
}