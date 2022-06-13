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
[Route("api/v{version:apiVersion}/cites")]
[ApiVersion("1.0")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;

    /// <summary>
    /// </summary>
    /// <param name="cityService"></param>
    public CityController(ICityService cityService)
    {
        _cityService = cityService;
    }

    /// <summary>
    ///     get all cities
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<CityResponseModel>> GetAll([FromQuery] CityParams param)
    {
        var entity = _cityService.GetAll(param);

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

        return ApiResponse<List<City>>.Success(entity, metadata);
    }

    /// <summary>
    ///     get city by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<CityResponseModel>> Get(int id)
    {
        var entity = await _cityService.Get(id);

        return ApiResponse<City>.Ok(entity);
    }

    /// <summary>
    ///     create city
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<CityResponseModel>> Post(CityRequestModel data)
    {
        var entity = await _cityService.CreateAsync(data);
        return ApiResponse<City>.Created(entity);
    }

    /// <summary>
    ///     update city
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ApiResponse<CityResponseModel>> Put([FromBody] CityRequestModel data)
    {
        var entity = await _cityService.UpdateAsync(data);
        return ApiResponse<City>.Created(entity);
    }

    /// <summary>
    ///     delete city
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<CityResponseModel>>> Delete(int id)
    {
        var entity = await _cityService.DeleteAsync(id);
        return ApiResponse<City>.Ok(entity);
    }
}