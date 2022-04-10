using System.ComponentModel.DataAnnotations;

namespace CityDiscoverTourist.Data.Models;

public class Contact
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? UrlSocial { get; set; }
    public string? Address { get; set; }

}