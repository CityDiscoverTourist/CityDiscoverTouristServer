using System.ComponentModel.DataAnnotations;

namespace CityDiscoverTourist.Data.Models;

public class Contact: BaseEntity
{
    public string? Name { get; set; }
    public string? UrlSocial { get; set; }
    public string? Address { get; set; }

}