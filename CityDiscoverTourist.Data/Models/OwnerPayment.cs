namespace CityDiscoverTourist.Data.Models;

public class OwnerPayment: BaseEntity
{
    public float TotalAmount { get; set; }
    public int TotalCount { get; set; }
    public float Commission { get; set; }
    public string? Status { get; set; }

    public OwnerPaymentPeriod? OwnerPaymentPeriod { get; set; }
    public int PaymentPeriodId { get; set; }

    public Quest? Quest { get; set; }
    public int QuestId { get; set; }

    public QuestOwner? Owner { get; set; }
    public int OwnerId { get; set; }

    public Transaction? Transaction { get; set; }
    public int TransactionId { get; set; }

}