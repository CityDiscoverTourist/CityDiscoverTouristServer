namespace CityDiscoverTourist.Business.Data.RequestModel;

public class WalletRequestModel
{
    public int Id { get; set; }
    public float Total { get; set; }
    public string? CurrencyUnit { get; set; }
}