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
public class OwnerPaymentPeriodController : ControllerBase
{
    private readonly IOwnerPaymentPeriodService _paymentPeriodService;

    public OwnerPaymentPeriodController(IOwnerPaymentPeriodService paymentPeriodService)
    {
        _paymentPeriodService = paymentPeriodService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<OwnerPaymentPeriod>> GetAll([FromQuery] OwnerPaymentPeriodParams param)
    {
        var entity = _paymentPeriodService.GetAll(param);

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

        return ApiResponse<List<OwnerPaymentPeriod>>.Ok2(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<OwnerPaymentPeriod>> Get(int id)
    {
        var entity = await _paymentPeriodService.Get(id);

        return ApiResponse<OwnerPaymentPeriod>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<OwnerPaymentPeriod>> Post(OwnerPaymentPeriod data)
    {
        var entity = await _paymentPeriodService.CreateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<OwnerPaymentPeriod>> Put([FromBody] OwnerPaymentPeriod data)
    {
        var entity = await _paymentPeriodService.UpdateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<OwnerPaymentPeriod>>> Delete(int id)
    {
        var entity = await _paymentPeriodService.DeleteAsync(id);
        return ApiResponse<Quest>.Ok(entity);
    }

}