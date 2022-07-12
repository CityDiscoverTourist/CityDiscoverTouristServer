using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
[Authorize]
public class CustomerAnswerController : ControllerBase
{
    private readonly ICustomerAnswerService _customerAnswerService;

    /// <summary>
    /// </summary>
    /// <param name="customerAnswerService"></param>
    public CustomerAnswerController(ICustomerAnswerService customerAnswerService)
    {
        _customerAnswerService = customerAnswerService;
    }

    /// <summary>
    ///     Get all customer answers
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    //[Cached(600)]
    public ApiResponse<PageList<CustomerAnswerResponseModel>> GetAll([FromQuery] CustomerAnswerParams param)
    {
        var entity = _customerAnswerService.GetAll(param);

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

        return ApiResponse<List<CustomerAnswer>>.Success(entity, metadata);
    }

    /// <summary>
    ///     Get customer answer by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    //[Cached(600)]
    public async Task<ApiResponse<CustomerAnswerResponseModel>> Get(int id)
    {
        var entity = await _customerAnswerService.Get(id);

        return ApiResponse<CustomerAnswer>.Ok(entity);
    }

    /// <summary>
    /// </summary>
    /// <param name="customerTaskId"></param>
    /// <returns></returns>
    [HttpGet("get-by-customer-task-id/{customerTaskId:int}")]
    public async Task<ApiResponse<List<CustomerAnswerResponseModel>>> GetByCustomerTaskId(int customerTaskId)
    {
        var entity = await _customerAnswerService.GetByCustomerTaskId(customerTaskId);

        return ApiResponse<CustomerAnswer>.Ok(entity);
    }

    /// <summary>
    ///     Create a new CustomerAnswer
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<CustomerAnswerResponseModel>> Post(CustomerAnswerRequestModel data)
    {
        var entity = await _customerAnswerService.CreateAsync(data);
        return ApiResponse<CustomerAnswer>.Created(entity);
    }

    /// <summary>
    ///     Update a customer answer
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ApiResponse<CustomerAnswerResponseModel>> Put(CustomerAnswerRequestModel data)
    {
        var entity = await _customerAnswerService.UpdateAsync(data);
        return ApiResponse<CustomerAnswer>.Created(entity);
    }

    /// <summary>
    ///     Delete a customer answer
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<CustomerAnswerResponseModel>>> Delete(int id)
    {
        var entity = await _customerAnswerService.DeleteAsync(id);
        return ApiResponse<CustomerAnswer>.Ok(entity);
    }
}