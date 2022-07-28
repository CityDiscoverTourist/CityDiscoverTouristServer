namespace CityDiscoverTourist.Business.Helper.Params;

public class PaymentParams : QueryStringParams
{
    public string? PaymentMethod { get; set; }
    public int CustomerQuestId { get; set; }
    public string? CustomerEmail { get; set; }
    public bool? IsValid { get; set; }
}