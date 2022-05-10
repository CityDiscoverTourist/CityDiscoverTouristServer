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
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<PaymentRequestModel>> GetAll([FromQuery] PaymentParams param)
    {
        var entity = _paymentService.GetAll(param);

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

        return ApiResponse<List<Payment>>.Ok2(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<PaymentRequestModel>> Get(int id)
    {
        var entity = await _paymentService.Get(id);

        return ApiResponse<Payment>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<PaymentRequestModel>> Post(PaymentRequestModel data)
    {
        var entity = await _paymentService.CreateAsync(data);
        return ApiResponse<Payment>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<PaymentRequestModel>> Put([FromBody] PaymentRequestModel data)
    {
        var entity = await _paymentService.UpdateAsync(data);
        return ApiResponse<Payment>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<PaymentRequestModel>>> Delete(int id)
    {
        var entity = await _paymentService.DeleteAsync(id);
        return ApiResponse<Payment>.Ok(entity);
    }

}