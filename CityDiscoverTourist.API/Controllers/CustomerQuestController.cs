using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            entity.HasPrevious,
        };
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

        return ApiResponse<List<CustomerQuestResponseModel>>.Success(entity, metadata);
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

    [HttpPut("update-end-point/{customerQuestId:int}")]
    public async Task<ApiResponse<CustomerQuestResponseModel>> UpdateEndPoint(int customerQuestId)
    {
        var entity = await _customerQuestService.UpdateEndPointAndStatusWhenFinishQuestAsync(customerQuestId);
        return ApiResponse<CustomerQuestResponseModel>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<CustomerQuestResponseModel>>> Delete(int id)
    {
        var entity = await _customerQuestService.DeleteAsync(id);
        return ApiResponse<CustomerQuestResponseModel>.Ok(entity);
    }

}