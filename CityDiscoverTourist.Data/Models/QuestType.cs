using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class QuestType: BaseEntity
{
    public string? Name { get; set; }
    public string? Status { get; set; }
    public string? ImagePath { get; set; }

    public string? DurationMode { get; set; }
    public string? DistanceMode { get; set; }
    public List<Quest>? Quests { get; set; }
    public List<Commission>? Commissions { get; set; }
}