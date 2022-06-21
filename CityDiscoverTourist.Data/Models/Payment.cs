namespace CityDiscoverTourist.Data.Models;

public class Payment: BaseEntity
{
    public string? PaymentMethod { get; set; }
    public string? Status { get; set; }
    public int Quantity { get; set; }
    public float TotalAmount { get; set; }

    public CustomerQuest? CustomerQuest { get; set; }
    public int CustomerQuestId { get; set; }
}