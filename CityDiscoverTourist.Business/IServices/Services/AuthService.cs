using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CityDiscoverTourist.Business.Data;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Data.Models;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CityDiscoverTourist.Business.IServices.Services;

public class AuthService: IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private static  IConfiguration? _configuration;


    public AuthService(UserManager<ApplicationUser> userManager, IConfiguration? configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<LoginResponseModel> LoginFirebase(LoginFirebaseModel model)
    {
        var userViewModel = await VerifyFirebaseToken(model.TokenId);
        var user = await _userManager.FindByNameAsync(userViewModel.Email);

        if (await CreateUserIfNotExits(user, userViewModel)) return null!;

        //if (user is {LockoutEnabled: false }) throw new ResponseModelException(ResponseCode.Forbidden, "User is locked");
        //if (!user.LockoutEnabled) throw new ResponseModelException(ResponseCode.Forbidden, "User is locked");
        var authClaims = new List<Claim>
        {
            new (ClaimTypes.Name, userViewModel.Email ?? string.Empty),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.Email, userViewModel.Email ?? string.Empty),
            new (ClaimTypes.Expiration, DateTime.Now.AddHours(1).ToString(CultureInfo.CurrentCulture)),
        };

        var accessToken = GetJwtToken(authClaims);

        userViewModel.JwtToken = new JwtSecurityTokenHandler().WriteToken(accessToken);
        userViewModel.RefreshToken = GenerateRefreshToken();
        userViewModel.RefreshTokenExpiryTime = DateTime.Now.AddSeconds(7);
        return userViewModel;
    }

    private async Task<bool> CreateUserIfNotExits(ApplicationUser user, LoginResponseModel userViewModel)
    {
        if (user != null) return false;
        user = new ApplicationUser()
        {
            UserName = userViewModel.Email,
            Email = userViewModel.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            EmailConfirmed = true,
            NormalizedEmail = userViewModel.Email?.ToUpper(),
            NormalizedUserName = userViewModel.FullName?.ToUpper(),
            PhoneNumberConfirmed = false,
        };
        var loginInfo = new ExternalLoginInfo(new ClaimsPrincipal(), "Firebase-Email", userViewModel.IdProvider, userViewModel.Email);
        var result = await _userManager.CreateAsync(user);
        await _userManager.AddLoginAsync(user, loginInfo);
        return !result.Succeeded;
    }

    public JwtSecurityToken GetJwtToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration!["JWT:Secret"]));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["JWT:ValidIssuer"],
            _configuration["JWT:ValidAudience"],
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signingCredentials
        );

        return token;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private static async Task<LoginResponseModel> VerifyFirebaseToken(string? token)
    {
        var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);

        var uid = decodedToken.Uid;
        var user = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
        // Query account table in DB

        var loginViewModel = new LoginResponseModel
        {
            IdProvider = uid,
            Email = user.Email,
            FullName = user.DisplayName,
            ImagePath = user.PhotoUrl
        };
        return loginViewModel;
    }
}