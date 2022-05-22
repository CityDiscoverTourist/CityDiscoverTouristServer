using CityDiscoverTourist.API.Response;
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
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<CustomerResponseModel>> GetAll([FromQuery] CustomerParams param)
    {
        var entity = _customerService.GetAll(param);

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

        return ApiResponse<List<CustomerResponseModel>>.Success(entity, metadata);
    }
    [HttpGet($"{{id}}")]
    //[Cached(600)]
    public async Task<ApiResponse<CustomerResponseModel>> Get(string id)
    {
        var entity = await _customerService.Get(id);

        return ApiResponse<ApplicationUser>.Ok(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<CustomerResponseModel>> Put([FromBody] ApplicationUser data)
    {
        var entity = await _customerService.UpdateAsync(data);
        return ApiResponse<ApplicationUser>.Created(entity);
    }
}