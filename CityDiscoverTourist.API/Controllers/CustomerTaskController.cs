using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CityDiscoverTourist.API.Controllers;

/// <inheritdoc />
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
//[Authorize]
public class CustomerTaskController : ControllerBase
{
    private readonly ICustomerTaskService _customerTaskService;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="taskService"></param>
    public CustomerTaskController(ICustomerTaskService taskService)
    {
        _customerTaskService = taskService;
    }

    /// <summary>
    ///     Get all tasks
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<CustomerTaskResponseModel>> GetAll([FromQuery] CustomerTaskParams param)
    {
        var entity = _customerTaskService.GetAll(param).Result;

        var metadata = new
        {
            entity.TotalCount,
            entity.TotalPages,
            entity.PageSize,
            entity.CurrentPage,
            entity.HasNext,
            entity.HasPrevious
        };
        return ApiResponse<List<CustomerTask>>.Success(entity, metadata);
    }

    /// <summary>
    ///     get customer task by customer quest id for admin
    /// </summary>
    /// <param name="param"></param>
    /// <param name="customerQuestId"></param>
    /// <returns></returns>
    [HttpGet("get-by-customer-quest-id/{customerQuestId:int}")]
    public ApiResponse<PageList<CustomerTaskResponseModel>> GetByCustomerQuestId(int customerQuestId,
        [FromQuery] CustomerTaskParams param)
    {
        var entity = _customerTaskService.GetByCustomerQuestId(customerQuestId, param).Result;

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
        return ApiResponse<List<CustomerTask>>.Success(entity, metadata);
    }

    /// <summary>
    ///     get customer task by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<CustomerTaskResponseModel>> Get(int id)
    {
        var entity = await _customerTaskService.Get(id);

        return ApiResponse<CustomerTaskResponseModel>.Ok(entity);
    }

    /// <summary>
    ///  customer want to skip task
    /// </summary>
    /// <returns></returns>
    [HttpPut("skip")]
    //[Cached(600)]
    public async Task<ApiResponse<CustomerTaskResponseModel>> Skip(int questItemId, int customerQuestId)
    {
        var entity = await _customerTaskService.Skip(questItemId, customerQuestId);

        return ApiResponse<CustomerTaskResponseModel>.Ok(entity);
    }

    /// <summary>
    ///     Get suggestions for the customer
    /// </summary>
    /// <param name="questItemId"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet("show-suggestion/{questItemId:int}")]
    public async Task<string> GetSuggestion(int questItemId, Language language)
    {
        var entity = await _customerTaskService.ShowSuggestions(questItemId, language);

        return JsonConvert.SerializeObject(entity);
    }

    /// <summary>
    ///     check customer location with quest item location if true allow to start quest item
    /// </summary>
    /// <param name="customerQuestId"></param>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <returns></returns>
    /// <exception cref="AppException"></exception>
    [HttpGet("check-location-with-quest-item/{customerQuestId:int}")]
    public Task<bool> CheckCustomerLocationWithQuestItem(int customerQuestId, float latitude, float longitude)
    {
        var isCustomerAtQuestItemLocation =
            _customerTaskService.IsCustomerAtQuestItemLocation(customerQuestId, latitude, longitude);
        if (!isCustomerAtQuestItemLocation) throw new AppException("Customer is not at quest item location");
        return Task.FromResult(isCustomerAtQuestItemLocation);
    }

    /// <summary>
    ///     check customer location with quest location if true allow to start quest
    /// </summary>
    /// <param name="questId"></param>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <returns></returns>
    /// <exception cref="AppException"></exception>
    [HttpGet("check-location-with-quest/{questId:int}")]
    public Task<bool> CheckCustomerLocationWithQuest(int questId, float latitude, float longitude)
    {
        var isCustomerAtQuestLocation =
            _customerTaskService.CheckCustomerLocationWithQuestLocation(questId, latitude, longitude);
        if (!isCustomerAtQuestLocation) throw new AppException("Customer is not at quest location");
        return Task.FromResult(isCustomerAtQuestLocation);
    }

    /// <summary>
    ///     add customer task for the first time when customer start quest
    /// </summary>
    /// <param name="data"></param>
    /// <param name="questId"></param>
    /// <returns></returns>
    [HttpPost("{questId:int}")]
    public async Task<ApiResponse<CustomerTaskResponseModel>> Post(CustomerTaskRequestModel data, int questId)
    {
        var entity = await _customerTaskService.CustomerStartQuest(data, questId);
        return ApiResponse<CustomerTaskResponseModel>.Created(entity);
    }

    /// <summary>
    ///     if customer correct or they skip the answer then move to next quest item
    /// </summary>
    /// <param name="questId"></param>
    /// <param name="customerQuestId"></param>
    /// <returns></returns>
    [HttpPut("move-next-task")]
    public async Task<int> MoveToNextTask(int questId, int customerQuestId)
    {
        var entity = await _customerTaskService.MoveCustomerToNextTask(questId, customerQuestId);
        return entity;
    }

    /// <summary>
    ///     decrease customer point when they hit suggestion
    /// </summary>
    /// <param name="customerQuestId"></param>
    /// <returns></returns>
    [HttpPut("decrease-point-suggestion/{customerQuestId:int}")]
    public async Task<ApiResponse<CustomerTaskResponseModel>> DecreasePointWhenHitSuggestion(int customerQuestId)
    {
        var entity = await _customerTaskService.DecreasePointWhenHitSuggestion(customerQuestId);
        return ApiResponse<CustomerTaskResponseModel>.Ok(entity);
    }

    /// <summary>
    ///     check customer answer if correct then call MoveToNextTask method, if not then decrease point
    /// </summary>
    /// <param name="customerQuestId"></param>
    /// <param name="customerReply"></param>
    /// <param name="questItemId"></param>
    /// <param name="files"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpPut("check-answer/{customerQuestId:int}")]
    public async Task<ApiResponse<CustomerTaskResponseModel>> CheckCustomerAnswer(int customerQuestId,
        string customerReply, int questItemId , [FromForm] List<IFormFile>? files = null, Language language = Language.vi)
    {
        var entity = await _customerTaskService.CheckCustomerAnswer(customerQuestId, customerReply, questItemId, files, language);
        return ApiResponse<CustomerTaskResponseModel>.Ok(entity);
    }

    /// <summary>
    ///     delete customer task
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<CustomerTaskResponseModel>>> Delete(int id)
    {
        var entity = await _customerTaskService.DeleteAsync(id);
        return ApiResponse<CustomerTaskResponseModel>.Ok(entity);
    }
}