namespace CityDiscoverTourist.Data.Models;

public class ActivityLog
{
    public int Id { get; set; }

    public ApplicationUser Customer { get; set; }
    public string? UserId { get; set; }

    public string? Action { get; set; }
    public string? Controller { get; set; }
    public string? Description { get; set; }
    public string? IpAddress { get; set; }
    public DateTime CreatedDate { get; set; }
}