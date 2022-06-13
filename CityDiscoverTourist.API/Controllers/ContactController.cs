using CityDiscoverTourist.API.Response;
using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    /// <summary>
    /// </summary>
    /// <param name="contactService"></param>
    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    /// <summary>
    ///     get all contacts
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public  Task<ApiResponse<IQueryable<Contact>>> GetAll()
    {
        var contacts = _contactService.GetContactAsync();
        return Task.FromResult(ApiResponse<Contact>.Ok(contacts));
    }

    /// <summary>
    ///     get contact by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    //[Cached(600)]
    public async Task<ApiResponse<Contact>> Get(int id)
    {
        var entity = await _contactService.Get(id);

        return ApiResponse<Contact>.Ok(entity);
    }

    /// <summary>
    ///     create contact
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ApiResponse<Contact>> Post(Contact data)
    {
        var entity = await _contactService.CreateAsync(data);
        return ApiResponse<Contact>.Created(entity);
    }

    /// <summary>
    ///     update contact
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ApiResponse<Contact>> Put(Contact data)
    {
        var entity = await _contactService.UpdateAsync(data);
        return ApiResponse<Contact>.Created(entity);
    }

    /// <summary>
    ///     delete a contact
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<Contact>>> Delete(int id)
    {
        var entity = await _contactService.DeleteAsync(id);
        return ApiResponse<Contact>.Ok(entity);
    }
}