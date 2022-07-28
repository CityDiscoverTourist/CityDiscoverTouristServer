namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class CustomerQuestResponseModel
{
    public int Id { get; set; }
    public string? BeginPoint { get; set; }
    public string? EndPoint { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int Rating { get; set; }
    public string? FeedBack { get; set; }

    public string? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public bool IsFinished { get; set; }

    public int QuestId { get; set; }
    public Guid PaymentId { get; set; }
    public string? Status { get; set; }
    public bool IsFeedbackApproved { get; set; }
    public Guid RewardCode { get; set; }
    public int PercentDiscount { get; set; }
    public double PercentPointRemain { get; set; }

    //public Payment? PaymentMethod { get; set; }
}