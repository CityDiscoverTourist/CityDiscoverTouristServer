namespace CityDiscoverTourist.Data.Models;

public class NotifyUser
{
    public ApplicationUser? User { get; set; }
    public string? UserId { get; set; }

    public Notification? Notification { get; set; }
    public int NotifyId { get; set; }

    public bool HasRead { get; set; }
}