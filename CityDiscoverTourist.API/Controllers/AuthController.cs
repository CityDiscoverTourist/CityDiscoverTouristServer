using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IFacebookService _facebookService;

    // GET
    public AuthController(IAuthService authService, IFacebookService facebookService)
    {
        _authService = authService;
        _facebookService = facebookService;
    }

    [HttpPost("login-firebase")]
    public async Task<ActionResult<LoginResponseModel>> LoginFirebase(LoginFirebaseModel model)
    {
        return Ok(await _authService.LoginFirebase(model));
    }

    [HttpPost("login-admin")]
    public async Task<ActionResult<LoginResponseModel>> LoginAdmin(LoginRequestModel model)
    {
        return Ok(await _authService.LoginForAdmin(model));
    }

    //register user
    [HttpPost("register-admin-account")]
    public async Task<ActionResult> Register(LoginRequestModel model)
    {
        return Ok(await _authService.Register(model));
    }

    [HttpGet("confirm-email")]
    public async Task<ActionResult> ConfirmEmail(string userId, string token)
    {
        return Ok(await _authService.ConfirmEmail(userId, token));
    }

    [HttpPost("login-facebook")]
    public async Task<IActionResult> FacebookLoginAsync([FromQuery] string resource)
    {
        var authorizationTokens = await _facebookService.LoginFacebookAsync(resource);
        return Ok(authorizationTokens);
    }
}