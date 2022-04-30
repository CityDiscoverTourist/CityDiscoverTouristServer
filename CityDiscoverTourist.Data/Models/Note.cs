namespace CityDiscoverTourist.Data.Models;

public class Note: BaseEntity
{
    public string? Content { get; set; }
    public string? Image { get; set; }

    public CustomerTask? CustomerTask { get; set; }
    public int CustomerTaskId { get; set; }
}