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
public class QuestOwnerController : ControllerBase
{
    private readonly IQuestOwnerService _questOwnerService;

    public QuestOwnerController(IQuestOwnerService questOwnerService)
    {
        _questOwnerService = questOwnerService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<QuestOwnerResponseModel>> GetAll([FromQuery] QuestOwnerParams param)
    {
        var entity = _questOwnerService.GetAll(param);

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

        return ApiResponse<List<QuestOwner>>.Success(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<QuestOwnerResponseModel>> Get(int id)
    {
        var entity = await _questOwnerService.Get(id);

        return ApiResponse<QuestOwner>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<QuestOwnerResponseModel>> Post(QuestOwnerRequestModel data)
    {
        var entity = await _questOwnerService.CreateAsync(data);
        return ApiResponse<QuestOwner>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<QuestOwnerResponseModel>> Put([FromBody] QuestOwnerRequestModel data)
    {
        var entity = await _questOwnerService.UpdateAsync(data);
        return ApiResponse<QuestOwner>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<QuestOwnerResponseModel>>> Delete(int id)
    {
        var entity = await _questOwnerService.DeleteAsync(id);
        return ApiResponse<QuestOwner>.Ok(entity);
    }
}