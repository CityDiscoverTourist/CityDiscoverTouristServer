namespace CityDiscoverTourist.Business.Data.RequestModel;

public class PaymentRequestModel
{
    public string? PaymentMethod { get; set; }
    public string? Status { get; set; }
    public int Quantity { get; set; }
    public float TotalAmount { get; set; }

    public string? CustomerId { get; set; }
    public int QuestId { get; set; }
}