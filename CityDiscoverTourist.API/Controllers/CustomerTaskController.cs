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

namespace CityDiscoverTourist.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class CustomerTaskController : ControllerBase
{
    private readonly ICustomerTaskService _customerTaskService;

    public CustomerTaskController(ICustomerTaskService taskService)
    {
        _customerTaskService = taskService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<CustomerTaskResponseModel>> GetAll([FromQuery] CustomerTaskParams param)
    {
        var entity = _customerTaskService.GetAll(param);

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

        return ApiResponse<List<CustomerTask>>.Success(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<CustomerTaskResponseModel>> Get(int id)
    {
        var entity = await _customerTaskService.Get(id);

        return ApiResponse<CustomerTaskResponseModel>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<CustomerTaskResponseModel>> Post(CustomerTaskRequestModel data)
    {
        var entity = await _customerTaskService.CreateAsync(data);
        return ApiResponse<CustomerTaskResponseModel>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<CustomerTaskResponseModel>> Put(CustomerTaskRequestModel data)
    {
        var entity = await _customerTaskService.UpdateAsync(data);
        return ApiResponse<CustomerTaskResponseModel>.Created(entity);
    }

    [HttpPut("update-current-point")]
    public async Task<ApiResponse<CustomerTaskResponseModel>> UpdateCurrentPoint(int id, float currentPoint)
    {
        var entity = await _customerTaskService.UpdateCurrentPointAsync(id, currentPoint);
        return ApiResponse<CustomerTaskResponseModel>.Created(entity);
    }

    [HttpPut("update-status")]
    public async Task<ApiResponse<CustomerTaskResponseModel>> UpdateStatus(int id, string status)
    {
        var entity = await _customerTaskService.UpdateStatusAsync(id, status);
        return ApiResponse<CustomerTaskResponseModel>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<CustomerTaskResponseModel>>> Delete(int id)
    {
        var entity = await _customerTaskService.DeleteAsync(id);
        return ApiResponse<CustomerTaskResponseModel>.Ok(entity);
    }
}