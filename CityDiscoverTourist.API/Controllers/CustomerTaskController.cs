using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
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

    [HttpGet("show-suggestion/{questItemId:int}")]
    public async Task<string> GetSuggestion(int questItemId)
    {
        var entity = await _customerTaskService.ShowSuggestions(questItemId);

        return JsonConvert.SerializeObject(entity);
    }

    [HttpGet("check-location/{customerQuestId:int}")]
    public Task<bool> CheckCustomerLocation(int customerQuestId, float latitude, float longitude)
    {
        return Task.FromResult(_customerTaskService.IsCustomerAtQuestItemLocation(customerQuestId, latitude, longitude));
    }

    [HttpPost("{questId:int}")]
    public async Task<ApiResponse<CustomerTaskResponseModel>> Post(CustomerTaskRequestModel data, int questId)
    {
        var entity = await _customerTaskService.CustomerStartQuest(data, questId);
        return ApiResponse<CustomerTaskResponseModel>.Created(entity);
    }

    [HttpPut("move-next-task")]
    public async Task<int> MoveToNextTask(int questId, int customerQuestId)
    {
        var entity = await _customerTaskService.MoveCustomerToNextTask(questId, customerQuestId);
        return entity;
    }

    [HttpPut("decrease-point-suggestion/{customerQuestId:int}")]
    public async Task<ApiResponse<CustomerTaskResponseModel>> DecreasePointWhenHitSuggestion(int customerQuestId)
    {
        var entity = await _customerTaskService.DecreasePointWhenHitSuggestion(customerQuestId);
        return ApiResponse<CustomerTaskResponseModel>.Ok(entity);
    }

    [HttpPut("check-answer/{customerQuestId:int}")]
    public async Task<ApiResponse<CustomerTaskResponseModel>> CheckCustomerAnswer(int customerQuestId, string customerReply, int questItemId)
    {
        var entity = await _customerTaskService.CheckCustomerAnswer(customerQuestId, customerReply, questItemId);
        return ApiResponse<CustomerTaskResponseModel>.Ok(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<CustomerTaskResponseModel>>> Delete(int id)
    {
        var entity = await _customerTaskService.DeleteAsync(id);
        return ApiResponse<CustomerTaskResponseModel>.Ok(entity);
    }
}