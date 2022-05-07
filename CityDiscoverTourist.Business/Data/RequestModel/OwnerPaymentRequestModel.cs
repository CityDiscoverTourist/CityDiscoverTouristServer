namespace CityDiscoverTourist.Business.Data.RequestModel;

public class OwnerPaymentRequestModel
{
    public int Id { get; set; }
    public float TotalAmount { get; set; }
    public int TotalCount { get; set; }
    public float Commission { get; set; }
    public string? Status { get; set; }

    public int PaymentPeriodId { get; set; }

    public int QuestId { get; set; }

    public int OwnerId { get; set; }

    public int TransactionId { get; set; }
}