namespace CityDiscoverTourist.Data.Models;

public class City: BaseEntity
{
    public string? Name { get; set; }
    public int Status { get; set; }

    public List<Area>? Areas { get; set; }
}