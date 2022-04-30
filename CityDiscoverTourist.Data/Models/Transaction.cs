namespace CityDiscoverTourist.Data.Models;

public class Transaction: BaseEntity
{
    public float Total { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? TypeTransaction { get; set; }
    public string? Status { get; set; }

    public Wallet? Wallet { get; set; }
    public int WalletId { get; set; }

    public OwnerPayment? OwnerPayments { get; set; }
}