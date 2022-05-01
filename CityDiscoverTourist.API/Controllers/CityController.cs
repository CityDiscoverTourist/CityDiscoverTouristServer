using AutoMapper;
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
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;

    public CityController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<City>> GetAll([FromQuery] CityParams param)
    {
        var entity = _cityService.GetAll(param);

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

        return ApiResponse<List<City>>.Ok2(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<City>> Get(int id)
    {
        var entity = await _cityService.Get(id);

        return ApiResponse<City>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<City>> Post(City data)
    {
        var entity = await _cityService.CreateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<City>> Put([FromBody] City data)
    {
        var entity = await _cityService.UpdateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<City>>> Delete(int id)
    {
        var entity = await _cityService.DeleteAsync(id);
        return ApiResponse<Quest>.Ok(entity);
    }

}