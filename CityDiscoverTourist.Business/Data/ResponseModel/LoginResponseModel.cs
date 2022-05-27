namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class LoginResponseModel
{
    public string? IdProvider { get; set; }
    public string? JwtToken { get; set; }
    public string? Email { get; set; }
    public string? AccountId { get; set; }
    public string? FullName { get; set; }
    public string? ImagePath { get; set; }
    public string? RefreshToken { get ; set ; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}