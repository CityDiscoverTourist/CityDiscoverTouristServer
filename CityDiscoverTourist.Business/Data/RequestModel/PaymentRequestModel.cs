namespace CityDiscoverTourist.Business.Data.RequestModel;

public class PaymentRequestModel
{
    public int Id { get; set; }
    public string? PaymentMethod { get; set; }
    public int Quantity { get; set; }
    public double AmountTotal { get; set; }
    public string? Status { get; set; }

    public int CustomerQuestId { get; set; }
}