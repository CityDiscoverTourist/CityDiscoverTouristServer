using Microsoft.AspNetCore.Identity;

namespace CityDiscoverTourist.Data.Models;

public class ApplicationUser: IdentityUser
{
     public string? ImagePath { get; set; }
     public List<CustomerQuest>? CustomerQuests { get; set; }

     public List<ActivityLog>? ActivityLogs { get; set; }
     public List<Reward>? Rewards { get; set; }
}