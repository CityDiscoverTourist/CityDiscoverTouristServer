using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class Reward: BaseEntity
{
    public Guid Code { get; set; }
    public string? Name { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public DateTime? ExpiredDate { get; set; }

    public ApplicationUser? Customer { get; set; }
    public string? CustomerId { get; set; }
    public int PercentDiscount { get; set; }

    /*public QuestReward? QuestReward { get; set; }
    public Guid QuestRewardId { get; set; }*/

    public Payment? Payment { get; set; }

    public string? Status { get; set; }
}