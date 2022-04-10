using System.ComponentModel.DataAnnotations;

namespace CityDiscoverTourist.Data.Models;

public class Quest
{
    [Key]
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public float Price { get; set; }
    public string? EstimateTime { get; set; }
    public DateTime AvailableTime { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string? Status { get; set; }

    public QuestType? QuestType { get; set; }
    public int QuestTypeId { get; set; }

    public List<Task>? Tasks { get; set; }
    public List<FeedBack>? FeedBack { get; set; }

    public List<Location>? Locations { get; set; }
}