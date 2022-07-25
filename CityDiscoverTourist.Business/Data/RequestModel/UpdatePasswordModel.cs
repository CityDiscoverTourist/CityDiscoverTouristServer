namespace CityDiscoverTourist.Business.Data.RequestModel;

public class UpdatePasswordModel
{
    public string? CustomerId { get; set; }
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
}