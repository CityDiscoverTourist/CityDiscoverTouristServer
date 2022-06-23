using CityDiscoverTourist.Business.IServices;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
///
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class UserSubscribedController : ControllerBase
{
    private readonly IUserSubscribedService _userSubscribedService;

    public UserSubscribedController(IUserSubscribedService userSubscribedService)
    {
        _userSubscribedService = userSubscribedService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userSubscribed"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<UserSubscribed> Post([FromBody] UserSubscribed userSubscribed)
    {
        return _userSubscribedService.CreateAsync(userSubscribed);
    }
}