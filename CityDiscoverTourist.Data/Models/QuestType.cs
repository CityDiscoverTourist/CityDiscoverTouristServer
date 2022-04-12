using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class QuestType: BaseEntity
{
    public string? Name { get; set; }
    public string? Status { get; set; }
    public List<Quest>? Quests { get; set; }
}