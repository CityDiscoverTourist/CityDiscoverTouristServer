using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CityDiscoverTourist.Business.Data;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.IServices;

public interface IAuthService
{
    public Task <LoginResponseModel> LoginFirebase(LoginFirebaseModel model);
    public JwtSecurityToken GetJwtToken(IEnumerable<Claim> claims);
    public string GenerateRefreshToken();
    public Task<bool> VerifyPhoneNumberByDigitCode(string username, int code);
}