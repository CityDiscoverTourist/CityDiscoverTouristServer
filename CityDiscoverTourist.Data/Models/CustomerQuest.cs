namespace CityDiscoverTourist.Data.Models;

public class CustomerQuest: BaseEntity
{
    public string? BeginPoint { get; set; }
    public string? EndPoint { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int Rating { get; set; }
    public string? FeedBack { get; set; }
    public string? TeamCode { get; set; }

    public ApplicationUser? Customer { get; set; }
    public string? CustomerId { get; set; }

    public Competition? Competition { get; set; }
    public int CompetitionId { get; set; }

    public Payment? PaymentMethod { get; set; }
    public List<CustomerTask>? CustomerTasks { get; set; }
}