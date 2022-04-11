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
public class ExperienceController : ControllerBase
{
    private readonly IExperienceService _experienceService;

    public ExperienceController(IExperienceService taskService)
    {
        _experienceService = taskService;
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<ExperienceResponseModel>> Get(int id, string? fields)
    {
        var entity = await _experienceService.Get(id, fields);

        return ApiResponse<Experience>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<ExperienceResponseModel>> Post(ExperienceRequestModel data)
    {
        var entity = await _experienceService.CreateAsync(data);
        return ApiResponse<Experience>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<ExperienceResponseModel>> Put(ExperienceRequestModel data)
    {
        var entity = await _experienceService.UpdateAsync(data);
        return ApiResponse<Experience>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<ExperienceResponseModel>>> Delete(int id)
    {
        var entity = await _experienceService.DeleteAsync(id);
        return ApiResponse<Experience>.Ok(entity);
    }

}