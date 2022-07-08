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

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class SuggestionController : ControllerBase
{
    private readonly ISuggestionService _suggestionService;

    /// <summary>
    /// </summary>
    /// <param name="suggestionService"></param>
    public SuggestionController(ISuggestionService suggestionService)
    {
        _suggestionService = suggestionService;
    }

    /// <summary>
    ///     get all suggestions
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<SuggestionResponseModel>> GetAll([FromQuery] SuggestionParams param)
    {
        var entity = _suggestionService.GetAll(param);

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

        return ApiResponse<List<Suggestion>>.Success(entity, metadata);
    }

    /// <summary>
    ///     get suggestion by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<SuggestionResponseModel>> Get(int id)
    {
        var entity = await _suggestionService.Get(id);

        return ApiResponse<Suggestion>.Ok(entity);
    }

    /// <summary>
    ///     create suggestion
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<SuggestionResponseModel>> Post(SuggestionRequestModel data)
    {
        var entity = await _suggestionService.CreateAsync(data);
        return ApiResponse<Suggestion>.Created(entity);
    }

    /// <summary>
    ///     update suggestion
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ApiResponse<SuggestionResponseModel>> Put([FromBody] SuggestionRequestModel data)
    {
        var entity = await _suggestionService.UpdateAsync(data);
        return ApiResponse<Suggestion>.Created(entity);
    }

    /// <summary>
    ///     soft  delete suggestion
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<SuggestionResponseModel>>> Delete(int id)
    {
        var entity = await _suggestionService.DeleteAsync(id);
        return ApiResponse<Suggestion>.Ok(entity);
    }

    /// <summary>
    ///     disable suggestion
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("disable/{id:int}")]
    public async Task<ActionResult<ApiResponse<SuggestionResponseModel>>> Disable(int id)
    {
        var entity = await _suggestionService.DisableAsync(id);
        return ApiResponse<Suggestion>.Ok(entity);
    }

    /// <summary>
    ///     Enable suggestion
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("enable/{id:int}")]
    public async Task<ActionResult<ApiResponse<SuggestionResponseModel>>> Enable(int id)
    {
        var entity = await _suggestionService.EnableAsync(id);
        return ApiResponse<Suggestion>.Ok(entity);
    }
}