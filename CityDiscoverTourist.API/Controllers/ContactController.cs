using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public  Task<ApiResponse<IQueryable<Contact>>> GetAll()
    {
        var contacts = _contactService.GetContactAsync();
        return Task.FromResult(ApiResponse<Contact>.Ok(contacts));
    }

    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<Contact>> Get(int id)
    {
        var entity = await _contactService.Get(id);

        return ApiResponse<Contact>.Ok(entity);
    }

    [HttpPost]
    public async Task<ApiResponse<Contact>> Post(Contact data)
    {
        var entity = await _contactService.CreateAsync(data);
        return ApiResponse<Contact>.Created(entity);
    }

    [HttpPut]
    public async Task<ApiResponse<Contact>> Put(Contact data)
    {
        var entity = await _contactService.UpdateAsync(data);
        return ApiResponse<Contact>.Created(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<Contact>>> Delete(int id)
    {
        var entity = await _contactService.DeleteAsync(id);
        return ApiResponse<Contact>.Ok(entity);
    }
}