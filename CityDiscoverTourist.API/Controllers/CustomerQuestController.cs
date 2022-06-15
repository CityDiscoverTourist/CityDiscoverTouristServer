using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class CustomerQuestController : ControllerBase
{
    private readonly ICustomerQuestService _customerQuestService;

    /// <summary>
    /// </summary>
    /// <param name="customerQuestService"></param>
    public CustomerQuestController(ICustomerQuestService customerQuestService)
    {
        _customerQuestService = customerQuestService;
    }

    /// <summary>
    ///     get all customer quest
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<CustomerQuestResponseModel>> GetAll([FromQuery] CustomerQuestParams param)
    {
        var entity = _customerQuestService.GetAll(param);

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

        return ApiResponse<List<CustomerQuestResponseModel>>.Success(entity, metadata);
    }


    /// <summary>
    ///     get customer quest by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<CustomerQuestResponseModel>> Get(int id)
    {
        var entity = await _customerQuestService.Get(id);

        return ApiResponse<CustomerQuestResponseModel>.Ok(entity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("get-by-customer-id")]
    //[Cached(600)]
    public async Task<ApiResponse<List<CustomerQuestResponseModel>>> GetByCustomerId(string id)
    {
        var entity = await _customerQuestService.GetByCustomerId(id);

        return ApiResponse<CustomerQuestResponseModel>.Ok(entity);
    }

    /// <summary>
    ///     create customer quest
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<CustomerQuestResponseModel>> Post(CustomerQuestRequestModel data)
    {
        var entity = await _customerQuestService.CreateAsync(data);
        return ApiResponse<CustomerQuestResponseModel>.Created(entity);
    }

    /// <summary>
    ///     update customer end point when customer finish quest
    /// </summary>
    /// <param name="customerQuestId"></param>
    /// <returns></returns>
    [HttpPut("update-end-point/{customerQuestId:int}")]
    public async Task<ApiResponse<CustomerQuestResponseModel>> UpdateEndPoint(int customerQuestId)
    {
        var entity = await _customerQuestService.UpdateEndPointAndStatusWhenFinishQuestAsync(customerQuestId);
        return ApiResponse<CustomerQuestResponseModel>.Created(entity);
    }

    /// <summary>
    ///     delete customer quest
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<CustomerQuestResponseModel>>> Delete(int id)
    {
        var entity = await _customerQuestService.DeleteAsync(id);
        return ApiResponse<CustomerQuestResponseModel>.Ok(entity);
    }
}