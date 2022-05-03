namespace CityDiscoverTourist.Data.Models;

public class Area: BaseEntity
{
    public string? Name { get; set; }
    public string? Status { get; set; }

    public City? City { get; set; }
    public int CityId { get; set; }

    public List<Quest>? Quests { get; set; }
    public List<Location>? Locations { get; set; }
}