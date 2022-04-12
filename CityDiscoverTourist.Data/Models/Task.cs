using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class Task: BaseEntity
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }
    public int Duration { get; set; }
    public int Experience { get; set; }
    public string? UrlStory { get; set; }

    public TaskType? TaskType { get; set; }
    public int TaskTypeId { get; set; }

    public Quest? Quest { get; set; }
    public Guid QuestId { get; set; }

    public Answer? Answer { get; set; }

    public string? Status { get; set; }
    public List<Suggestion>? Suggestions { get; set; }
}