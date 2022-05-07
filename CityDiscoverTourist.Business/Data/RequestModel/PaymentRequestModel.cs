namespace CityDiscoverTourist.Business.Data.RequestModel;

public class PaymentRequestModel
{
    public int Id { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Status { get; set; }

    public int CustomerQuestId { get; set; }
}