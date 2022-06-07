using System.Net.Http.Headers;
using CityDiscoverTourist.API.Cache;
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
public class AreaController : ControllerBase
{
    private readonly IAreaService _areaService;

    public AreaController(IAreaService areaService)
    {
        _areaService = areaService;
    }
    [HttpGet("nganluong_4a26a97795b77d23ed6ec4abc3e53360aa.html")]
    public ContentResult ConfirmVerify()
    {
        var html = System.IO.File.ReadAllText("./nganluong_4a26a97795b77d23ed6ec4abc3e53360.html");
        return base.Content(html, "text/html");
    }

    [HttpGet("nganluong_4a26a97795b77d23ed6ec4abc3e53360.html")]
    public ContentResult ConfirmVerify2()
    {
        var html = System.IO.File.ReadAllText($"https://citytouriststorage.blob.core.windows.net/quest/nganluong_b33f67a3f413f9def8a0722681c96348.html");
        return base.Content(html, "text/html");
    }
    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<AreaResponseModel>> GetAll([FromQuery] AreaParams param)
    {
        var entity = _areaService.GetAll(param);

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

        return ApiResponse<List<AreaResponseModel>>.Success(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<AreaResponseModel>> Get(int id)
    {
        var entity = await _areaService.Get(id);

        return ApiResponse<Area>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<AreaResponseModel>> Post(AreaRequestModel data)
    {
        var entity = await _areaService.CreateAsync(data);
        return ApiResponse<Area>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<AreaResponseModel>> Put([FromBody] AreaRequestModel data)
    {
        var entity = await _areaService.UpdateAsync(data);
        return ApiResponse<Area>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<AreaResponseModel>>> Delete(int id)
    {
        var entity = await _areaService.DeleteAsync(id);
        return ApiResponse<Area>.Ok(entity);
    }

}