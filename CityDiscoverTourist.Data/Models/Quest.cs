using System.ComponentModel.DataAnnotations;

namespace CityDiscoverTourist.Data.Models;

public class Quest
{
    [Key]
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public float Price { get; set; }
    public string? EstimatedTime { get; set; }
    public string? EstimatedDistance { get; set; }
    public DateTime? AvailableTime { get; set; } = null;

    public DateTime? CreatedDate { get; }
    public DateTime? UpdatedDate { get; }
    public string? Status { get; set; }

    public QuestType? QuestType { get; set; }
    public int QuestTypeId { get; set; }

    public List<Task>? Tasks { get; set; }

    public List<FeedBack>? FeedBack { get; set; }
    //public List<CustomerQuest>? CustomerQuests { get; set; }
    public List<Location>? Locations { get; set; }

    /*public ApplicationUser Customer { get; set; }
    public string CustomerId { get; set; }*/
    public Quest()
    {
        CreatedDate = DateTime.UtcNow.Date;
        UpdatedDate = DateTime.UtcNow.Date;
    }
}