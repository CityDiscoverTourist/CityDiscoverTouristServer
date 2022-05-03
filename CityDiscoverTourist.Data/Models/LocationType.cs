namespace CityDiscoverTourist.Data.Models;

public class LocationType: BaseEntity
{
    public string? Name { get; set; }
    public string? Status { get; set; }

    public List<Location>? Locations { get; set; }
}