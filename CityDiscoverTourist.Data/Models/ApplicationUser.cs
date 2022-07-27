using Microsoft.AspNetCore.Identity;

namespace CityDiscoverTourist.Data.Models;

public class ApplicationUser: IdentityUser
{
     public string? FullName { get; set; }
     public string? Address { get; set; }
     public bool Gender { get; set; }
     public string? ConfirmToken { get; set; }
     public string? ImagePath { get; set; }
     public List<CustomerQuest>? CustomerQuests { get; set; }
     public List<Payment>? Payments { get; set; }

     public List<ActivityLog>? ActivityLogs { get; set; }
     public List<Reward>? Rewards { get; set; }
     public IList<NotifyUser> NotifyUsers { get; set; }

}