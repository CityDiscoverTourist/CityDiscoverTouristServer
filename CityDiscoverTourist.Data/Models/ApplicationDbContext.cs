using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CityDiscoverTourist.Data.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Quest> Quests { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<QuestType> QuestTypes { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}