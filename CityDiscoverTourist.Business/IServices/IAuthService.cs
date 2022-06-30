using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;

namespace CityDiscoverTourist.Business.IServices;

public interface IAuthService
{
    public Task <LoginResponseModel> LoginFirebase(LoginFirebaseModel model);
    public Task <LoginResponseModel> LoginMobile(LoginFirebaseModel model);
    public Task<LoginResponseModel> LoginForAdmin(LoginRequestModel model);
    public  Task<bool> Register(LoginRequestModel model);
    public Task<string> ConfirmEmail(string userId, string token);
    public JwtSecurityToken GetJwtToken(IEnumerable<Claim> claims);
    public string GenerateRefreshToken();
    public Task CreateRole();
}