using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CityDiscoverTourist.Data.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Quest> Quests { get; set; }
    public DbSet<QuestType> QuestTypes { get; set; }
    public DbSet<Suggestion> Suggestions { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }
    public DbSet<Reward> Rewards { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<CustomerAnswer> CustomerAnswers { get; set; }
    public DbSet<CustomerTask> CustomerTasks { get; set; }
    public DbSet<CustomerQuest> CustomerQuests { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Commission> Commissions { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<LocationType> LocationTypes { get; set; }
    public DbSet<OwnerPayment> OwnerPayments { get; set; }
    public DbSet<OwnerPaymentPeriod> OwnerPaymentPeriods { get; set; }
    public DbSet<QuestItem> QuestItems { get; set; }
    public DbSet<QuestItemType> QuestItemTypes { get; set; }
    public DbSet<QuestOwner> QuestOwners { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<QuestReward> QuestRewards { get; set; }
    public DbSet<UserSubscribed> UserSubscribeds { get; set; }
}