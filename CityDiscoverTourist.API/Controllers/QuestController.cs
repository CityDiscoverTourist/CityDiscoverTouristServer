using AutoMapper;
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
public class QuestController : ControllerBase
{
    private readonly IQuestService _questService;

    public QuestController(IQuestService questService)
    {
        _questService = questService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<QuestResponseModel>> GetTutorRequest([FromQuery] QuestParams param)
    {
        var entity = _questService.GetAll(param);

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

        return ApiResponse<List<QuestResponseModel>>.Ok2(entity, metadata);
    }

    [HttpGet("{id:guid}")]
    //[Cached(600)]

    public async Task<ApiResponse<QuestResponseModel>> Get(Guid id, string? fields)
    {
        var entity = await _questService.Get(id, fields);

        return ApiResponse<QuestResponseModel>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<QuestResponseModel>> Post(QuestRequestModel data)
    {
        var entity = await _questService.CreateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<QuestResponseModel>> Put(QuestRequestModel data)
    {
        var entity = await _questService.UpdateAsync(data);
        return ApiResponse<Quest>.Created(entity);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<QuestResponseModel>>> Delete(Guid id)
    {
        var entity = await _questService.DeleteAsync(id);
        return ApiResponse<Quest>.Ok(entity);
    }

}