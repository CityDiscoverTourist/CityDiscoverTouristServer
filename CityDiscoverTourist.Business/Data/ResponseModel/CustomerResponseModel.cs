using Microsoft.AspNetCore.Identity;

namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class CustomerResponseModel : IdentityUser
{
    public string? ImagePath { get; set; }
    public bool? Gender { get; set; }
    public string? Address { get; set; }
    public string? FullName { get; set; }
}