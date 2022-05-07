namespace CityDiscoverTourist.Business.Data.RequestModel;

public class TransactionRequestModel
{
    public int Id { get; set; }
    public float Total { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? TypeTransaction { get; set; }
    public string? Status { get; set; }

    public int WalletId { get; set; }
}