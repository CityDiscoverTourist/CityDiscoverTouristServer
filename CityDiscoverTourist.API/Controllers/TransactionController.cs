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
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transService;

    public TransactionController(ITransactionService transService)
    {
        _transService = transService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<TransactionResponseModel>> GetAll([FromQuery] TransactionParams param)
    {
        var entity = _transService.GetAll(param);

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

        return ApiResponse<List<Transaction>>.Success(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<TransactionResponseModel>> Get(int id)
    {
        var entity = await _transService.Get(id);

        return ApiResponse<Transaction>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<TransactionResponseModel>> Post(TransactionRequestModel data)
    {
        var entity = await _transService.CreateAsync(data);
        return ApiResponse<Transaction>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<TransactionResponseModel>> Put([FromBody] TransactionRequestModel data)
    {
        var entity = await _transService.UpdateAsync(data);
        return ApiResponse<Transaction>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<TransactionResponseModel>>> Delete(int id)
    {
        var entity = await _transService.DeleteAsync(id);
        return ApiResponse<Transaction>.Ok(entity);
    }
}