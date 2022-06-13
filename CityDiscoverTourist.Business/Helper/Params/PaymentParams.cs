namespace CityDiscoverTourist.Business.Helper.Params;

public class PaymentParams : QueryStringParams
{
    public string? PaymentMethod { get; set; }
    public int CustomerQuestId { get; set; }
}