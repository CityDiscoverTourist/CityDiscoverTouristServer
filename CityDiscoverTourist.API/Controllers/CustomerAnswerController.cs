using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Business.IServices.Services;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Answer = CityDiscoverTourist.Data.Models.Answer;

namespace CityDiscoverTourist.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class CustomerAnswerController : ControllerBase
{
    private readonly ICustomerAnswerService _customerAnswerService;

    public CustomerAnswerController(ICustomerAnswerService customerAnswerService)
    {
        _customerAnswerService = customerAnswerService;
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<CustomerAnswer>> Get(int id)
    {
        var entity = await _customerAnswerService.Get(id);

        return ApiResponse<CustomerAnswer>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<CustomerAnswer>> Post(CustomerAnswer data)
    {
        var entity = await _customerAnswerService.CreateAsync(data);
        return ApiResponse<CustomerAnswer>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<CustomerAnswer>> Put(CustomerAnswer data)
    {
        var entity = await _customerAnswerService.UpdateAsync(data);
        return ApiResponse<CustomerAnswer>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<CustomerAnswer>>> Delete(int id)
    {
        var entity = await _customerAnswerService.DeleteAsync(id);
        return ApiResponse<CustomerAnswer>.Ok(entity);
    }

}