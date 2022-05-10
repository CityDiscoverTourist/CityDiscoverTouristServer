namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class OwnerPaymentResponseModel
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