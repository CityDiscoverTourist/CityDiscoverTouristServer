using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
//[Authorize]
public class CustomerQuestController : ControllerBase
{
    private readonly ICustomerQuestService _customerQuestService;
    private readonly IRecurringJobManager _recurringJobManager;

    /// <summary>
    /// </summary>
    /// <param name="customerQuestService"></param>
    /// <param name="recurringJobManager"></param>
    public CustomerQuestController(ICustomerQuestService customerQuestService, IRecurringJobManager recurringJobManager)
    {
        _customerQuestService = customerQuestService;
        _recurringJobManager = recurringJobManager;
    }

    /// <summary>
    ///     get all customer quest
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
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

        return ApiResponse<List<CustomerQuestResponseModel>>.Success(entity, metadata);
    }

    /// <summary>
    ///     get customer quest by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    //[Cached(600)]
    public async Task<ApiResponse<CustomerQuestResponseModel>> Get(int id)
    {
        var entity = await _customerQuestService.Get(id);

        return ApiResponse<CustomerQuestResponseModel>.Ok(entity);
    }

    /// <summary>
    ///     get customer comment by quest id
    /// </summary>
    /// <param name="questId"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet("show-comments/{questId:int}")]
    //[Cached(600)]
    [AllowAnonymous]
    public async Task<ApiResponse<PageList<CommentResponseModel>>> GetComments(int questId,
        [FromQuery] CustomerQuestParams param)
    {
        var entity = await _customerQuestService.ShowComments(questId, param);

        var metadata = new
        {
            entity.TotalCount,
            entity.TotalPages,
            entity.PageSize,
            entity.CurrentPage,
            entity.HasNext,
            entity.HasPrevious
        };

        return ApiResponse<CommentResponseModel>.Success(entity, metadata);
    }

    /// <summary>
    ///     get customer quest by customer id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [HttpGet("get-by-customer-id")]
    //[Cached(600)]
    [AllowAnonymous]
    public async Task<ApiResponse<List<CustomerQuestResponseModel>>> GetByCustomerId(string id, Language language = Language.vi)
    {
        var entity = await _customerQuestService.GetByCustomerId(id, language);

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
    ///     feedback for customer quest when finish quest
    /// </summary>
    /// <param name="id"></param>
    /// <param name="comment"></param>
    /// <returns></returns>
    [HttpPost("feed-back/{id:int}")]
    public async Task<ApiResponse<CustomerQuestResponseModel>> GiveFeedback(int id,
        [FromBody] CommentRequestModel comment)
    {
        var entity = await _customerQuestService.GiveFeedback(id, comment);
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
    ///     back ground job to update status
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    public async Task<OkObjectResult> Put()
    {
        _recurringJobManager.AddOrUpdate("CustomerQuest", () => _customerQuestService.InvalidCustomerQuest(),
            "0 0 * * *", TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        return await Task.FromResult(Ok("RecurringJobManager"));
    }

    /// <summary>
    /// </summary>
    /// <param name="customerQuestId"></param>
    /// <param name="comment"></param>
    /// <returns></returns>
    [HttpPut("update-comment")]
    public async Task<ApiResponse<List<CommentResponseModel>>> UpdateComment(int customerQuestId,
        CommentRequestModel comment)
    {
        var entity = await _customerQuestService.UpdateComment(customerQuestId, comment);
        return ApiResponse<CustomerQuestResponseModel>.Created(entity);
    }

    /// <summary>
    ///  approve customer feedback
    /// </summary>
    /// <param name="id"></param>
    [HttpPut("approve-feedback/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task ApproveFeedback(int id)
    {
        await _customerQuestService.ApproveFeedback(id);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <param name="forceDelete"></param>
    [HttpPut("force-delete/{id:int}")]
    public async Task ForceDelete(int id, bool forceDelete)
    {
        await _customerQuestService.ForceDelete(id, forceDelete);
    }


    /// <summary>
    /// </summary>
    /// <param name="questId"></param>
    /// <param name="customerId"></param>
    /// <returns></returns>
    [HttpGet("get-comment")]
    public Task<ApiResponse<IQueryable<CommentResponseModel>>> GetMyComment(int questId, string customerId)
    {
        var entity = _customerQuestService.GetMyComment(questId, customerId);
        return Task.FromResult(ApiResponse<CommentResponseModel>.Created(entity));
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