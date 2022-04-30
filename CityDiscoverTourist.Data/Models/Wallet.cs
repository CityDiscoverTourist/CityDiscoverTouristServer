namespace CityDiscoverTourist.Data.Models;

public class Wallet: BaseEntity
{
    public float Total { get; set; }
    public string? CurrencyUnit { get; set; }

    public List<Transaction>? Transactions { get; set; }

    public QuestOwner? QuestOwner { get; set; }
}