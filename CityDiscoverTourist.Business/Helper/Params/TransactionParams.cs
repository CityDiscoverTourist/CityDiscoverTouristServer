namespace CityDiscoverTourist.Business.Helper.Params;

public class TransactionParams : QueryStringParams
{
    public float Total { get; set; }
    public string? Type { get; set; }
    public int WalletId { get; set; }
}