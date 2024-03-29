using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Data.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace CityDiscoverTourist.Business.IServices.Services;

public class FacebookService : BaseService, IFacebookService
{
    private const string FacebookUri = "https://graph.facebook.com/v14.0/";
    private readonly IAuthService _authService;

    private readonly HttpClient _httpClient;
    private readonly UserManager<ApplicationUser> _userManager;

    public FacebookService(UserManager<ApplicationUser> userManager, IAuthService authService)
    {
        _userManager = userManager;
        _authService = authService;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(FacebookUri)
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<FacebookResponseModel> GetUserFromFacebookAsync(string facebookToken)
    {
        var result = await GetAsync<dynamic>(facebookToken, "me",
            "scope=public_profile&fields=first_name,last_name,email,id,picture.width(100).height(100)");
        if (result == null) throw new KeyNotFoundException("Invalid facebook token");

        var account = new FacebookResponseModel
        {
            FullName = result.first_name + " " + result.last_name,
            ImagePath = result.picture.data.url,
            Email = result.email,
            FacebookId = result.id
        };

        return account;
    }

    public async Task<T> GetAsync<T>(string accessToken, string endpoint, string? args = null)
    {
        var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(result)!;
    }

    public async Task<LoginResponseModel> LoginFacebookAsync(string token, string deviceId)
    {
        await _authService.CreateRole();
        if (string.IsNullOrEmpty(token)) throw new KeyNotFoundException("Token is null");

        var facebookUser = await GetUserFromFacebookAsync(token);
        var userDb = await _userManager.FindByEmailAsync(facebookUser.Email) ?? await CreateUserIfNotExits(facebookUser);

        // check if user is customer
        if (!await _userManager.IsInRoleAsync(userDb!, Role.Customer.ToString()))
            throw new UnauthorizedAccessException("Account not allowed to login");

        if (userDb is {LockoutEnabled: false }) throw new AppException("User is locked");

        userDb.DeviceId = deviceId;
        await _userManager.UpdateAsync(userDb);

        var authClaims = new List<Claim>
        {
            new (ClaimTypes.Name, facebookUser.FullName!),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.Email, facebookUser.Email!),
            new (ClaimTypes.Role, Role.Customer.ToString()),
            new (ClaimTypes.Expiration, CurrentDateTime().AddHours(4).ToString(CultureInfo.CurrentCulture))
        };

        var accessToken = _authService.GetJwtToken(authClaims);

        //await _paymentService.PushNotification(userDb.DeviceId!, userDb.Id);

        return new LoginResponseModel
        {
            IdProvider = facebookUser.FacebookId,
            Email = facebookUser.Email,
            FullName = facebookUser.FullName,
            ImagePath = facebookUser.ImagePath,
            JwtToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = _authService.GenerateRefreshToken(),
            RefreshTokenExpiryTime = DateTime.Now.AddHours(1),
            AccountId = userDb.Id
        };
    }

    private async Task<ApplicationUser?> CreateUserIfNotExits(FacebookResponseModel facebookUser)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        ApplicationUser? user = null;
        if (user != null) return null;

        user = new ApplicationUser
        {
            UserName = facebookUser.Email,
            Email = facebookUser.Email,
            FullName = facebookUser.FullName,
            SecurityStamp = Guid.NewGuid().ToString(),
            EmailConfirmed = true,
            NormalizedEmail = facebookUser.Email!.ToUpper(),
            NormalizedUserName = facebookUser.FullName!.ToUpper(),
            PhoneNumberConfirmed = false,
            ImagePath = facebookUser.ImagePath
        };
        var loginInfo = new ExternalLoginInfo(new ClaimsPrincipal(), "Facebook", facebookUser.FacebookId, "Facebook");
        var result = await _userManager.CreateAsync(user);

        await _userManager.AddToRoleAsync(user, Role.Customer.ToString());

        await _userManager.AddLoginAsync(user, loginInfo);

        return result.Succeeded ? user : null;
    }
}