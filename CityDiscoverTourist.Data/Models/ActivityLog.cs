namespace CityDiscoverTourist.Data.Models;

public class ActivityLog: BaseEntity
{
    public string? Action { get; set; }
    public string? Controller { get; set; }
    public string? Description { get; set; }
    public string? IpAddress { get; set; }
    public DateTime? CreatedDate { get; set; } = null;

    public ApplicationUser? Customer { get; set; }
    public string? CustomerId { get; set; }
}