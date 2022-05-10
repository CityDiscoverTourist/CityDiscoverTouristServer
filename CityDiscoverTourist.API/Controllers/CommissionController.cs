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
public class CommissionController : ControllerBase
{
    private readonly ICommissionService _commissionService;

    public CommissionController(ICommissionService commissionService)
    {
        _commissionService = commissionService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<CommissionResponseModel>> GetAll([FromQuery] CommissionParams param)
    {
        var entity = _commissionService.GetAll(param);

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

        return ApiResponse<List<Commission>>.Ok2(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<CommissionResponseModel>> Get(int id)
    {
        var entity = await _commissionService.Get(id);

        return ApiResponse<Commission>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<CommissionResponseModel>> Post(CommissionRequestModel data)
    {
        var entity = await _commissionService.CreateAsync(data);
        return ApiResponse<Commission>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<CommissionResponseModel>> Put([FromBody] CommissionRequestModel data)
    {
        var entity = await _commissionService.UpdateAsync(data);
        return ApiResponse<Commission>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<CommissionResponseModel>>> Delete(int id)
    {
        var entity = await _commissionService.DeleteAsync(id);
        return ApiResponse<Commission>.Ok(entity);
    }

}