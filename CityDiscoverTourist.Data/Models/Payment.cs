using System.ComponentModel.DataAnnotations;

namespace CityDiscoverTourist.Data.Models;

public class Payment
{
    [Key]
    public Guid Id { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Status { get; set; }
    public int Quantity { get; set; }
    public float TotalAmount { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public List<CustomerQuest>? CustomerQuests { get; set; }

    public ApplicationUser? Customer { get; set; }
    public string? CustomerId { get; set; }
    public Quest? Quest { get; set; }
    public int QuestId { get; set; }

    public Reward? Reward { get; set; }
    public int? RewardId { get; set; }
    public bool IsValid { get; set; } = true;
}