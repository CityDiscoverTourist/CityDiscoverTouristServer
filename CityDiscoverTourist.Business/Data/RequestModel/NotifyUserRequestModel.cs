namespace CityDiscoverTourist.Business.Data.RequestModel;

public class NotifyUserRequestModel
{
    public string? Content { get; set; }
    public DateTime CreatedDate { get; set; }
    public int QuestId { get; set; }
    public Guid PaymentId { get; set; }
}