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
public class LocationTypeController : ControllerBase
{
    private readonly ILocationTypeService _locationTypeService;

    public LocationTypeController(ILocationTypeService locationTypeService)
    {
        _locationTypeService = locationTypeService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<LocationTypeResponseModel>> GetAll([FromQuery] LocationTypeParams param)
    {
        var entity = _locationTypeService.GetAll(param);

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

        return ApiResponse<List<LocationType>>.Ok2(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<LocationTypeResponseModel>> Get(int id)
    {
        var entity = await _locationTypeService.Get(id);

        return ApiResponse<LocationType>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<LocationTypeResponseModel>> Post(LocationTypeRequestModel data)
    {
        var entity = await _locationTypeService.CreateAsync(data);
        return ApiResponse<LocationType>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<LocationTypeResponseModel>> Put([FromBody] LocationTypeRequestModel data)
    {
        var entity = await _locationTypeService.UpdateAsync(data);
        return ApiResponse<LocationType>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<LocationTypeResponseModel>>> Delete(int id)
    {
        var entity = await _locationTypeService.DeleteAsync(id);
        return ApiResponse<LocationType>.Ok(entity);
    }

}