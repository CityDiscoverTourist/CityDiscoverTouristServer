using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using CityDiscoverTourist.Business.Data.RequestModel;
using CityDiscoverTourist.Business.Data.ResponseModel;
using CityDiscoverTourist.Business.Enums;
using CityDiscoverTourist.Business.Exceptions;
using CityDiscoverTourist.Business.Helper.EmailHelper;
using CityDiscoverTourist.Data.Models;
using FirebaseAdmin.Auth;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PasswordGenerator;

namespace CityDiscoverTourist.Business.IServices.Services;

public class AuthService : BaseService, IAuthService
{
    private static  IConfiguration? _configuration;
    private readonly IEmailSender _emailSender;
    private  readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthService(UserManager<ApplicationUser> userManager, IConfiguration? configuration,
        RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _configuration = configuration;
        _roleManager = roleManager;
        _emailSender = emailSender;
    }

    public async Task<LoginResponseModel> LoginFirebase(LoginFirebaseModel model)
    {
        await CreateRole();
        var userViewModel = await VerifyFirebaseToken(model.TokenId);
        var user = await _userManager.FindByNameAsync(userViewModel.Email);

        // check if user is customer
        if (!await _userManager.IsInRoleAsync(user, Role.Customer.ToString()))
            throw new UnauthorizedAccessException("Account not allowed to login");

        if (await CreateUserIfNotExits(user, userViewModel)) return null!;

        if (user is {LockoutEnabled: false }) throw new AppException("User is locked");

        user.DeviceId = model.DeviceId;
        await _userManager.UpdateAsync(user);

        var authClaims = new List<Claim>
        {
            new (ClaimTypes.Name, userViewModel.Email ?? string.Empty),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.Email, userViewModel.Email ?? string.Empty),
            new (ClaimTypes.Role, Role.Customer.ToString()),
            new (ClaimTypes.Expiration, CurrentDateTime().AddHours(4).ToString(CultureInfo.InvariantCulture))
        };

        var accessToken = GetJwtToken(authClaims);

        userViewModel.JwtToken = new JwtSecurityTokenHandler().WriteToken(accessToken);
        userViewModel.RefreshToken = GenerateRefreshToken();
        userViewModel.RefreshTokenExpiryTime = DateTime.Now.AddSeconds(7);
        userViewModel.AccountId = user.Id;

        //await _paymentService.PushNotification(user.DeviceId!, user.Id);

        return userViewModel;
    }

    public async Task<LoginResponseModel> Login(LoginRequestModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null) throw new AppException("Customer not found");

        // check if user is customer
        if (!await _userManager.IsInRoleAsync(user, Role.Customer.ToString()))
            throw new UnauthorizedAccessException("Account not allowed to login");

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        if (!user.LockoutEnabled) throw new AppException("Customer is locked");

        if (!user.EmailConfirmed) throw new AppException("Customer is not confirmed");

        if (model.DeviceId != null)
        {
            user.DeviceId = model.DeviceId;
            await _userManager.UpdateAsync(user);
        }

        var authClaims = new List<Claim>
        {
            new (ClaimTypes.Name, user.Email ?? string.Empty),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.Email, user.Email ?? string.Empty),
            new (ClaimTypes.Role, Role.Customer.ToString()),
            new (ClaimTypes.Expiration, CurrentDateTime().AddHours(4).ToString(CultureInfo.CurrentCulture))
        };

        var accessToken = GetJwtToken(authClaims);

        var userViewModel = new LoginResponseModel
        {
            JwtToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = GenerateRefreshToken(),
            RefreshTokenExpiryTime = CurrentDateTime().AddDays(7),
            Email = user.Email,
            AccountId = user.Id,
            FullName = user.UserName
        };

        //await _paymentService.PushNotification(user.DeviceId!, user.Id);

        return userViewModel;
    }

    public async Task<LoginResponseModel> LoginAdmin(LoginRequestModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null) throw new AppException("Admin not found");

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        // check if user is admin
        if (!await _userManager.IsInRoleAsync(user, Role.Admin.ToString()))
            throw new UnauthorizedAccessException("Account not allowed to login");

        var authClaims = new List<Claim>
        {
            new (ClaimTypes.Name, user.Email ?? string.Empty),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.Email, user.Email ?? string.Empty),
            new (ClaimTypes.Role, Role.Admin.ToString()),
            new Claim (ClaimTypes.Expiration, CurrentDateTime().AddHours(3).ToString(CultureInfo.CurrentCulture))
        };

        var accessToken = GetJwtToken(authClaims);

        var userViewModel = new LoginResponseModel
        {
            JwtToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = GenerateRefreshToken(),
            RefreshTokenExpiryTime = CurrentDateTime().AddDays(7),
            Email = user.Email,
            AccountId = user.Id,
            FullName = user.UserName
        };
        //await _paymentService.PushNotification("dOJJNFd5SH-BEwUdgr8tpx:APA91bHV-yTd9jr7Ff_H9JqGQmkivnlcq69zxiCAfU7eon7NphKO3zfpz5aL65QNnO1k_uPfTAyFiWFDBgIwNa_yPGHVCt9qdwHiIFfB0bGRsCl3-zW947Di7EfOATzhwJgZ7hzySCha", "zxc");
        return userViewModel;
    }

    // register new user
    public async Task<bool> Register(LoginRequestModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is { }) throw new AppException("User already exists");

        user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = false,
            LockoutEnabled = false
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded) throw new AppException(result.Errors.First().Description);

        await _userManager.AddToRoleAsync(user, Role.Customer.ToString());

        // send mail to user with confirmation link to activate account
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var urlEncode = HttpUtility.UrlEncode(token);

        var confirmationLink =
            $"{_configuration!["AppUrl"]}/api/v1/auths/confirm-email?userId={user.Id}&token={urlEncode}";

        var message = "<h1>Welcome to City Discover Tourist</h1> <br/>" +
                      $"<p>Please confirm your account by clicking <a href='{confirmationLink}'>here</a></p>";

        //await _emailSender.SendMailConfirmAsync(user.Email!, "Confirm your account", message);

        BackgroundJob.Enqueue(() => _emailSender.SendMailConfirmAsync(user.Email!, "Confirm your account", message));

        user.ConfirmToken = token;
        await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    public async Task<string> ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new AppException("User not found");

        if (user.EmailConfirmed) throw new AppException("User already confirmed");

        if (user.ConfirmToken != token) throw new AppException("Invalid token");

        user.EmailConfirmed = true;
        user.ConfirmToken = null;
        await _userManager.UpdateAsync(user);

        // return html string
        return "<h1>Email confirmed</h1> <br/>" +
               "<p>You can now login to your account</p>";
    }

    public JwtSecurityToken GetJwtToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration!["JWT:Secret"]));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_configuration["JWT:ValidIssuer"], _configuration["JWT:ValidAudience"], claims,
            expires: DateTime.Now.AddHours(1), signingCredentials: signingCredentials);

        return token;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task CreateRole()
    {
        if (!_roleManager.RoleExistsAsync(Role.Admin.ToString()).GetAwaiter().GetResult())
        {
            await _roleManager.CreateAsync(new IdentityRole(Role.Admin.ToString()));
            await _roleManager.CreateAsync(new IdentityRole(Role.Customer.ToString()));
        }
    }

    public async Task ForgotPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        await _userManager.RemovePasswordAsync(user);

        if (user is null) throw new AppException("User not found");

        var newPassword = GeneratePassword();
        var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        var message = "<h1>Dear " + user.UserName + "</h1> <br/>" + "Your new password is: " + newPassword + "<br/>" +
                      "Please change your password after login" + "<br/>" + "Thank you";

        await _userManager.ResetPasswordAsync(user, passwordToken, newPassword);

        await _emailSender.SendMailConfirmAsync(email, "Forgot password", message);
    }

    private static string GeneratePassword()
    {
        var pwd = new Password(8).IncludeLowercase().IncludeUppercase().IncludeSpecial().IncludeNumeric();
        var password = pwd.Next();
        return password;
    }

    private async Task<bool> CreateUserIfNotExits(ApplicationUser user, LoginResponseModel userViewModel)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if (user != null) return false;
        user = new ApplicationUser
        {
            UserName = userViewModel.Email,
            Email = userViewModel.Email,
            FullName = userViewModel.FullName,
            SecurityStamp = Guid.NewGuid().ToString(),
            EmailConfirmed = true,
            NormalizedEmail = userViewModel.Email?.ToUpper(),
            NormalizedUserName = userViewModel.FullName?.ToUpper(),
            PhoneNumberConfirmed = false,
            ImagePath = userViewModel.ImagePath
        };
        var loginInfo = new ExternalLoginInfo(new ClaimsPrincipal(), "Firebase-Email", userViewModel.IdProvider,
            userViewModel.Email);
        var result = await _userManager.CreateAsync(user);

        await _userManager.AddToRoleAsync(user, Role.Customer.ToString());

        await _userManager.AddLoginAsync(user, loginInfo);

        return !result.Succeeded;
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