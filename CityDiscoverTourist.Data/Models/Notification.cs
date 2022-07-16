namespace CityDiscoverTourist.Data.Models;

public class Notification : BaseEntity
{
    public string? Content { get; set; }
    public DateTime CreatedDate { get; set; }
}