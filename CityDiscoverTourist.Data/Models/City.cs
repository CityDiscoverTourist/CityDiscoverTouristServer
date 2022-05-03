namespace CityDiscoverTourist.Data.Models;

public class City: BaseEntity
{
    public string? Name { get; set; }
    public string? Status { get; set; }

    public List<Area>? Areas { get; set; }
}