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
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<Wallet>> GetAll([FromQuery] WalletParams param)
    {
        var entity = _walletService.GetAll(param);

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

        return ApiResponse<List<Wallet>>.Ok2(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<Wallet>> Get(int id)
    {
        var entity = await _walletService.Get(id);

        return ApiResponse<Wallet>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<Wallet>> Post(Wallet data)
    {
        var entity = await _walletService.CreateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<Wallet>> Put([FromBody] Wallet data)
    {
        var entity = await _walletService.UpdateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<Wallet>>> Delete(int id)
    {
        var entity = await _walletService.DeleteAsync(id);
        return ApiResponse<Quest>.Ok(entity);
    }

}