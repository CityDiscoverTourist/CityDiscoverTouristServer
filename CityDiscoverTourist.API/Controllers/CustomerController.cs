using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
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
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    /// <summary>
    /// </summary>
    /// <param name="customerService"></param>
    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    ///     get all customers
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    //[Cached(600)]
    public async Task<ApiResponse<PageList<CustomerResponseModel>>> GetAll([FromQuery] CustomerParams param)
    {
        var entity = await _customerService.GetAll(param);

        var metadata = new
        {
            entity.TotalCount,
            entity.TotalPages,
            entity.PageSize,
            entity.CurrentPage,
            entity.HasNext,
            entity.HasPrevious
        };

        return ApiResponse<List<CustomerResponseModel>>.Success(entity, metadata);
    }

    /// <summary>
    ///     get customer by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    //[Cached(600)]
    public async Task<ApiResponse<CustomerResponseModel>> Get(string id)
    {
        var entity = await _customerService.Get(id);

        return ApiResponse<ApplicationUser>.Ok(entity);
    }

    /// <summary>
    ///     update customer
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ApiResponse<CustomerResponseModel>> Put([FromForm] CustomerRequestModel data)
    {
        var entity = await _customerService.UpdateAsync(data);
        return ApiResponse<ApplicationUser>.Created(entity);
    }

    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut("update-password")]
    public async Task<ApiResponse<CustomerResponseModel>> UpdatePassword([FromBody] UpdatePasswordModel data)
    {
        var entity = await _customerService.UpdatePassword(data);
        return ApiResponse<ApplicationUser>.Created(entity);
    }

    /// <summary>
    ///     update customer status, false = inactive, true = active
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isLock"></param>
    /// <returns></returns>
    [HttpPut("{id}/{isLock:bool}")]
    [Authorize(Roles = "Admin")]
    public async Task<ApiResponse<CustomerResponseModel>> UpdateLockUser(string id, bool isLock)
    {
        var entity = await _customerService.UpdateUser(id, isLock);
        return ApiResponse<ApplicationUser>.Created(entity);
    }
}