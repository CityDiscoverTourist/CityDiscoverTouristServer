namespace CityDiscoverTourist.Data.Models;

public class CustomerQuest: BaseEntity
{
    public string? BeginPoint { get; set; }
    public string? EndPoint { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int Rating { get; set; }
    public string? FeedBack { get; set; }
    public string? Status { get; set; }
    public bool IsFinished { get; set; }

    public Quest? Quest { get; set; }
    public int QuestId { get; set; }

    public Payment? PaymentMethod { get; set; }
    public Guid PaymentId { get; set; }

    public ApplicationUser? Customer { get; set; }
    public string? CustomerId { get; set; }
    public bool IsFeedbackApproved { get; set; } = true;

    public List<CustomerTask>? CustomerTasks { get; set; }
}