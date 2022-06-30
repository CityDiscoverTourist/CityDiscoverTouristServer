namespace CityDiscoverTourist.Business.Data.RequestModel;

public class PaymentRequestModel
{
    public Guid Id { get; set; }
    public string? PaymentMethod { get; set; }
    public int Quantity { get; set; }
    public double AmountTotal { get; set; }
    public string? Status { get; set; }
    public string? CustomerId { get; set; }

    public int QuestId { get; set; }
}