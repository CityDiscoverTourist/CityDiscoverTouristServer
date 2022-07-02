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
///
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;

    /// <summary>
    ///
    /// </summary>
    /// <param name="noteService"></param>
    public NoteController(INoteService noteService)
    {
        _noteService = noteService;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    //[Cached(600)]
    public ApiResponse<PageList<NoteResponseModel>> GetAll([FromQuery] NoteParams param)
    {
        var entity = _noteService.GetAll(param);

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

        return ApiResponse<List<Note>>.Success(entity, metadata);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<NoteResponseModel>> Get(int id)
    {
        var entity = await _noteService.Get(id);

        return ApiResponse<Note>.Ok(entity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<NoteResponseModel>> Post(NoteRequestModel data)
    {
        var entity = await _noteService.CreateAsync(data);
        return ApiResponse<Note>.Created(entity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ApiResponse<NoteResponseModel>> Put([FromBody] NoteRequestModel data)
    {
        var entity = await _noteService.UpdateAsync(data);
        return ApiResponse<Note>.Created(entity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<NoteResponseModel>>> Delete(int id)
    {
        var entity = await _noteService.DeleteAsync(id);
        return ApiResponse<Note>.Ok(entity);
    }
}