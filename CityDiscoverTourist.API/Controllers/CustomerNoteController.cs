using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Helper;
using CityDiscoverTourist.Business.Helper.Params;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Business.IServices.Services;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Task = CityDiscoverTourist.Data.Models.Task;

namespace CityDiscoverTourist.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class CustomerNoteController : ControllerBase
{
    private readonly INoteService _noteService;

    public CustomerNoteController(INoteService taskService)
    {
        _noteService = taskService;
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<Note>> Get(int id)
    {
        var entity = await _noteService.Get(id);

        return ApiResponse<Note>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<Note>> Post(Note data)
    {
        var entity = await _noteService.CreateAsync(data);
        return ApiResponse<Note>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<Note>> Put(Note data)
    {
        var entity = await _noteService.UpdateAsync(data);
        return ApiResponse<Note>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<Note>>> Delete(int id)
    {
        var entity = await _noteService.DeleteAsync(id);
        return ApiResponse<Note>.Ok(entity);
    }

}