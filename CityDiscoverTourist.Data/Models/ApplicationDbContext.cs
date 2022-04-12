using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CityDiscoverTourist.Data.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Quest> Quests { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<QuestType> QuestTypes { get; set; }
    public DbSet<TaskType> TaskTypes { get; set; }
    public DbSet<FeedBack> FeedBacks { get; set; }
    public DbSet<Suggestion> Suggestions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }
    public DbSet<Reward> Rewards { get; set; }
    public DbSet<Experience> Experiences { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Contact> Contacts { get; set; }
}