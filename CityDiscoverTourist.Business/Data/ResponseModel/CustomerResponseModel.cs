using Microsoft.AspNetCore.Identity;

namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class CustomerResponseModel : IdentityUser
{
    public string? ImagePath { get; set; }
}