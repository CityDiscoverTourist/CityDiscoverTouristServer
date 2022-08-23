using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IFacebookService _facebookService;

    /// <summary>
    /// </summary>
    /// <param name="authService"></param>
    /// <param name="facebookService"></param>
    public AuthController(IAuthService authService, IFacebookService facebookService)
    {
        _authService = authService;
        _facebookService = facebookService;
    }

    /// <summary>
    ///     login with gmail
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("login-firebase")]
    public async Task<ActionResult<LoginResponseModel>> LoginFirebase(LoginFirebaseModel model)
    {
        return Ok(await _authService.LoginFirebase(model));
    }

    /// <summary>
    ///     login with username and password for user
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
    {
        return Ok(await _authService.Login(model));
    }

    /// <summary>
    ///     login for admin
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("login-admin")]
    public async Task<ActionResult<LoginResponseModel>> LoginAdmin(LoginRequestModel model)
    {
        return Ok(await _authService.LoginAdmin(model));
    }

    //register user
    /// <summary>
    ///     register user
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("register-account")]
    public async Task<ActionResult> Register(LoginRequestModel model)
    {
        return Ok(await _authService.Register(model));
    }

    /// <summary>
    ///  register for admin
    /// </summary>
    /// <param name="model"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    [HttpPost("register-admin")]
    [Authorize(Roles = "Admin")]
    //[AllowAnonymous]
    public async Task<ActionResult> RegisterAdmin(LoginRequestModel model, Role role)
    {
        return Ok(await _authService.RegisterAdmin(model, role));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpPost("resend-mail")]
    public async Task ResendMail(string email)
    {
        await _authService.ReSendMail(email);
    }

    /// <summary>
    ///     confirm email for user register
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("confirm-email")]
    public async Task<ActionResult> ConfirmEmail(string userId, string token)
    {
        return Ok(await _authService.ConfirmEmail(userId, token));
    }

    /// <summary>
    ///     login with facebook
    /// </summary>
    /// <param name="resource"></param>
    /// <param name="deviceId"></param>
    /// <returns></returns>
    [HttpPost("login-facebook")]
    public async Task<IActionResult> FacebookLoginAsync([FromQuery] string resource, string? deviceId = null)
    {
        var authorizationTokens = await _facebookService.LoginFacebookAsync(resource, deviceId!);
        return Ok(authorizationTokens);
    }

    /// <summary>
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpPost("forgot-password")]
    public async Task<ActionResult> ForgotPassword(string email)
    {
        await _authService.ForgotPassword(email);
        return Ok();
    }
}