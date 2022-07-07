using System.ComponentModel.DataAnnotations;

namespace CityDiscoverTourist.Data.Models;

public class QuestReward
{
    [Key]
    public Guid Code { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? Status { get; set; }
    public int PercentDiscount { get; set; }
    public int PercentPointRemain { get; set; }

    /*public Quest? Quest { get; set; }
    public int QuestId { get; set; }

    public List<Reward>? Rewards { get; set; }*/
}