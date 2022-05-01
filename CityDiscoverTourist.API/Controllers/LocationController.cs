using CityDiscoverTourist.API.Response;
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
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<Location>> GetAll([FromQuery] LocationParams param)
    {
        var entity = _locationService.GetAll(param);

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

        return ApiResponse<List<Location>>.Ok2(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<Location>> Get(int id)
    {
        var entity = await _locationService.Get(id);

        return ApiResponse<Location>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<Location>> Post(Location data)
    {
        var entity = await _locationService.CreateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<Location>> Put([FromBody] Location data)
    {
        var entity = await _locationService.UpdateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<Location>>> Delete(int id)
    {
        var entity = await _locationService.DeleteAsync(id);
        return ApiResponse<Quest>.Ok(entity);
    }

}