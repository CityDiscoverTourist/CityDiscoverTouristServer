using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class Task: BaseEntity
{
    public string? Content { get; set; }
    public string? Description { get; set; }
    public int Duration { get; set; }
    public string? Address { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; } = null;

    public TaskType? TaskType { get; set; }
    public int TaskTypeId { get; set; }

    public Quest? Quest { get; set; }
    public Guid QuestId { get; set; }

    public Answer? Answer { get; set; }

    public string? Status { get; set; }
    public List<Suggestion> Suggestions { get; set; }
    public List<CustomerTask>? CustomerTasks { get; set; }
}