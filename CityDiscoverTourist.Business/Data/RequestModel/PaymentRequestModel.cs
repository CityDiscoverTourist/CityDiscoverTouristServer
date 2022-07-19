namespace CityDiscoverTourist.Business.Data.RequestModel;

public class PaymentRequestModel
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public float TotalAmount { get; set; }
    public string? CustomerId { get; set; }
    public bool IsMobile { get; set; }
    public int QuestId { get; set; }
}