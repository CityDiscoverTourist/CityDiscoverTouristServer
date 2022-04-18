using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Business.IServices.Services;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Quest = CityDiscoverTourist.Data.Models.Quest;

namespace CityDiscoverTourist.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class CustomerQuestController : ControllerBase
{
    private readonly ICustomerQuestService _customerQuestService;

    public CustomerQuestController(ICustomerQuestService customerQuestService)
    {
        _customerQuestService = customerQuestService;
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<CustomerQuestResponseModel>> Get(int id)
    {
        var entity = await _customerQuestService.Get(id);

        return ApiResponse<CustomerQuestResponseModel>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<CustomerQuestResponseModel>> Post(CustomerQuestRequestModel data)
    {
        var entity = await _customerQuestService.CreateAsync(data);
        return ApiResponse<CustomerQuestResponseModel>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<CustomerQuestResponseModel>> Put(CustomerQuestRequestModel data)
    {
        var entity = await _customerQuestService.UpdateAsync(data);
        return ApiResponse<CustomerQuestResponseModel>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<CustomerQuestResponseModel>>> Delete(int id)
    {
        var entity = await _customerQuestService.DeleteAsync(id);
        return ApiResponse<CustomerQuestResponseModel>.Ok(entity);
    }

}