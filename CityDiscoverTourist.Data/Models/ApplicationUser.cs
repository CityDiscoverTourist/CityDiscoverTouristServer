using Microsoft.AspNetCore.Identity;

namespace CityDiscoverTourist.Data.Models;

public class ApplicationUser: IdentityUser
{
     public List<ActivityLog>? ActivityLogs { get; set; }
     public List<Reward>? Rewards { get; set; }
     public List<Order>? Orders { get; set; }

     public Experience Experience { get; set; }
}