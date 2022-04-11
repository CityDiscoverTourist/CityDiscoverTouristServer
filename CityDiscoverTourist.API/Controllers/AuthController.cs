using CityDiscoverTourist.Business.Data;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    // GET
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("loginFirebase")]
    public async Task<ActionResult<LoginResponseModel>> LoginFirebase(LoginFirebaseModel model)
    {
        return Ok(await _authService.LoginFirebase(model));
    }

    [HttpPost("refresh")]
    public IActionResult Refresh(string accessToken)
    {
        var principal = _authService.GetPrincipalFromExpiredToken(accessToken);

        var newToken = _authService.GetJwtToken(principal.Claims);
        var newRefreshToken = _authService.GenerateRefreshToken();

        return Ok(new { token = newToken.EncodedPayload, newRefreshToken });
    }

    [HttpPost("confirm-phone")]
    public async Task<IActionResult> ConfirmPhone(string username, int code)
    {
        return Ok(await _authService.VerifyPhoneNumberByDigitCode(username, code));
    }
}