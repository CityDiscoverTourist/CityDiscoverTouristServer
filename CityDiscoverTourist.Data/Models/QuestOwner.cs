namespace CityDiscoverTourist.Data.Models;

public class QuestOwner: BaseEntity
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? Status { get; set; }

    public List<OwnerPayment>? OwnerPayments { get; set; }

    public Wallet? Wallet { get; set; }
    public int WalletId { get; set; }
}