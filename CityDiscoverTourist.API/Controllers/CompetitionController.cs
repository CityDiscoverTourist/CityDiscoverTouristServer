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
public class CompetitionController : ControllerBase
{
    private readonly ICompetitionService _competitionService;

    public CompetitionController(ICompetitionService competitionService)
    {
        _competitionService = competitionService;
    }

    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<CompetitionResponseModel>> GetAll([FromQuery] CompetitionParams param)
    {
        var entity = _competitionService.GetAll(param);

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

        return ApiResponse<List<Competition>>.Success(entity, metadata);
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]

    public async Task<ApiResponse<CompetitionResponseModel>> Get(int id)
    {
        var entity = await _competitionService.Get(id);

        return ApiResponse<Competition>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<CompetitionResponseModel>> Post(CompetitionRequestModel data)
    {
        var entity = await _competitionService.CreateAsync(data);
        return ApiResponse<Competition>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<CompetitionResponseModel>> Put([FromBody] CompetitionRequestModel data)
    {
        var entity = await _competitionService.UpdateAsync(data);
        return ApiResponse<Competition>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<CompetitionResponseModel>>> Delete(int id)
    {
        var entity = await _competitionService.DeleteAsync(id);
        return ApiResponse<Competition>.Ok(entity);
    }

}