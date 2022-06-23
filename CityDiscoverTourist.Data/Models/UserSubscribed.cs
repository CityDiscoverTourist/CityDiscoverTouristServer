namespace CityDiscoverTourist.Data.Models;

public class UserSubscribed : BaseEntity
{
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
}