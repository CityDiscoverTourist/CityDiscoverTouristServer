namespace CityDiscoverTourist.Business.Helper.Params;

public class PaymentParams : QueryStringParams
{
    public string? PaymentMethod { get; set; }
    public int CustomerQuestId { get; set; }
    public string? Status { get; set; }
    public string? CustomerId { get; set; }
    public bool? IsValid { get; set; }
}