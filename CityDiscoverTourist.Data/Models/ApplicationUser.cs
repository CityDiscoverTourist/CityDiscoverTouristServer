using Microsoft.AspNetCore.Identity;

namespace CityDiscoverTourist.Data.Models;

public class ApplicationUser: IdentityUser
{
     public List<ActivityLog>? ActivityLogs { get; set; }
     public List<Reward>? Rewards { get; set; }
     public List<FeedBack>? FeedBacks { get; set; }
     public List<CustomerTask>? CustomerTasks { get; set; }
     public List<CustomerQuest>? CustomerQuests { get; set; }
     public List<Quest>? QuestOwners { get; set; }

     public Experience? Experience { get; set; }
}