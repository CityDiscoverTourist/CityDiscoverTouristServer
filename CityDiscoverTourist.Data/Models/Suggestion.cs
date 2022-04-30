using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class Suggestion: BaseEntity
{
    public string? Content { get; set; }
    public string? Status { get; set; }

    public QuestItem? QuestItem { get; set; }
    public int QuestItemId { get; set; }
}