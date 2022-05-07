using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
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
public class OwnerPaymentController : ControllerBase
{
    private readonly IOwnerPaymentService _ownerPaymentService;

    public OwnerPaymentController(IOwnerPaymentService ownerPaymentService)
    {
        _ownerPaymentService = ownerPaymentService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<OwnerPayment>> GetAll([FromQuery] OwnerPaymentParams param)
    {
        var entity = _ownerPaymentService.GetAll(param);

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

        return ApiResponse<List<OwnerPayment>>.Ok2(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<OwnerPayment>> Get(int id)
    {
        var entity = await _ownerPaymentService.Get(id);

        return ApiResponse<OwnerPayment>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<OwnerPayment>> Post(OwnerPaymentRequestModel data)
    {
        var entity = await _ownerPaymentService.CreateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<OwnerPayment>> Put([FromBody] OwnerPaymentRequestModel data)
    {
        var entity = await _ownerPaymentService.UpdateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<OwnerPayment>>> Delete(int id)
    {
        var entity = await _ownerPaymentService.DeleteAsync(id);
        return ApiResponse<Quest>.Ok(entity);
    }

}