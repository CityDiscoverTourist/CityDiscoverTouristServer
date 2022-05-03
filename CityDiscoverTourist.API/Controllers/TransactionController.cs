using CityDiscoverTourist.API.Response;
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
    public ApiResponse<PageList<Transaction>> GetAll([FromQuery] TransactionParams param)
    {
        var entity = _transService.GetAll(param);

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

        return ApiResponse<List<Transaction>>.Ok2(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<Transaction>> Get(int id)
    {
        var entity = await _transService.Get(id);

        return ApiResponse<Transaction>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<Transaction>> Post(Transaction data)
    {
        var entity = await _transService.CreateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<Transaction>> Put([FromBody] Transaction data)
    {
        var entity = await _transService.UpdateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<Transaction>>> Delete(int id)
    {
        var entity = await _transService.DeleteAsync(id);
        return ApiResponse<Quest>.Ok(entity);
    }

}