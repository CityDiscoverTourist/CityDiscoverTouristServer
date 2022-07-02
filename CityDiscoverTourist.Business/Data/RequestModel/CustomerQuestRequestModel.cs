namespace CityDiscoverTourist.Business.Data.RequestModel;

public class CustomerQuestRequestModel
{
    public int Rating { get; set; }
    public string? FeedBack { get; set; }

    public string? CustomerId { get; set; }
    public int QuestId { get; set; }
    public Guid PaymentId { get; set; }
}