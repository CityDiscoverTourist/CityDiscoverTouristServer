using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    /// <summary>
    /// </summary>
    /// <param name="paymentService"></param>
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    /// </summary>
    /// <param name="param"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<PaymentResponseModel>> GetAll([FromQuery] PaymentParams param,
        Language language = Language.vi)
    {
        var entity = _paymentService.GetAll(param, language);

        var metadata = new
        {
            entity.TotalCount,
            entity.TotalPages,
            entity.PageSize,
            entity.CurrentPage,
            entity.HasNext,
            entity.HasPrevious
        };

        return ApiResponse<List<Payment>>.Success(entity, metadata);
    }

    /// <summary>
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
    /// </summary>
    /// <param name="params"></param>
    /// <param name="customerId"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet("get-by-customer-id")]
    //[Cached(600)]
    public async Task<ApiResponse<PageList<PaymentResponseModel>>> GetByCustomerId([FromQuery] PaymentParams @params,
        string customerId, Language language = Language.vi)
    {
        var entity = await _paymentService.GetByCustomerId(@params, customerId, language);

        return ApiResponse<Payment>.Ok(entity);
    }

    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    /// <param name="discountCode"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<string[]>> Post(PaymentRequestModel data, Guid discountCode)
    {
        var entity = await _paymentService.CreateAsync(data, discountCode);
        return ApiResponse<Payment>.Created(entity);
    }

    /// <summary>
    /// </summary>
    /// <param name="couponCode"></param>
    /// <param name="customerId"></param>
    /// <param name="totalPrice"></param>
    /// <returns></returns>
    [HttpPost("check-coupon")]
    public async Task<ApiResponse<string[]>> CheckCoupon(Guid couponCode, string customerId, float totalPrice)
    {
        var entity = await _paymentService.CheckCoupon(couponCode, customerId, totalPrice);
        return ApiResponse<Payment>.Created(entity);
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<OkObjectResult> Put()
    {
        //_recurringJobManager.AddOrUpdate("Payment", () => _paymentService.InvalidOrder(), "0 0 * * *", TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        await _paymentService.InvalidOrder();
        return await Task.FromResult(Ok("RecurringJobManager"));
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    [HttpPost("callback")]
    [AllowAnonymous]
    public Task<PaymentResponseModel> Callback([FromBody] MomoRequestModel dto)
    {
        return _paymentService.UpdateStatusWhenSuccess(dto);
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<PaymentResponseModel>>> Delete(Guid id)
    {
        var entity = await _paymentService.DeleteAsync(id);
        return ApiResponse<Payment>.Ok(entity);
    }
}