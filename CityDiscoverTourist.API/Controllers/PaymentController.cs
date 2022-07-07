using System.Web;
using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Data.Models;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
///
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IRecurringJobManager _recurringJobManager;

    /// <summary>
    ///
    /// </summary>
    /// <param name="paymentService"></param>
    /// <param name="recurringJobManager"></param>
    public PaymentController(IPaymentService paymentService, IRecurringJobManager recurringJobManager)
    {
        _paymentService = paymentService;
        _recurringJobManager = recurringJobManager;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<PaymentResponseModel>> GetAll([FromQuery] PaymentParams param)
    {
        var entity = _paymentService.GetAll(param);

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

        return ApiResponse<List<Payment>>.Success(entity, metadata);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet("{id:Guid}")]
    //[Cached(600)]
    public async Task<ApiResponse<PaymentResponseModel>> Get(Guid id, Language language = Language.vi)
    {
        var entity = await _paymentService.Get(id, language);

        return ApiResponse<Payment>.Ok(entity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    [HttpGet("get-by-customer-id")]
    //[Cached(600)]
    public async Task<ApiResponse<List<PaymentResponseModel>>> GetByCustomerId(string customerId)
    {
        var entity = await _paymentService.GetByCustomerId(customerId);

        return ApiResponse<Payment>.Ok(entity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="data"></param>
    /// <param name="discountCode"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<string>> Post(PaymentRequestModel data, Guid discountCode)
    {
        var entity = await _paymentService.CreateAsync(data, discountCode);
        return ApiResponse<Payment>.Created(entity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    public async Task<OkObjectResult> Put()
    {
        _recurringJobManager.AddOrUpdate(
            "Payment", () => _paymentService.InvalidOrder(),
            Cron.Daily(23, 55));
        return await Task.FromResult(Ok("RecurringJobManager"));
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [HttpPost("callback")]
    public Task<PaymentResponseModel> Callback([FromBody] MomoRequestModel dto)
    {
        return _paymentService.UpdateStatusWhenSuccess(dto);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult<ApiResponse<PaymentResponseModel>>> Delete(Guid id)
    {
        var entity = await _paymentService.DeleteAsync(id);
        return ApiResponse<Payment>.Ok(entity);
    }
}