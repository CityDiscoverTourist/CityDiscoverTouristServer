namespace CityDiscoverTourist.Data.Models;

public class Area: BaseEntity
{
    public string? Name { get; set; }
    public int Status { get; set; }

    public City? City { get; set; }
    public int CityId { get; set; }

    public Quest? Quest { get; set; }
    public int QuestId { get; set; }

    public List<Location>? Locations { get; set; }
}