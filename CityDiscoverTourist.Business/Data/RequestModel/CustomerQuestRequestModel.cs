namespace CityDiscoverTourist.Business.Data.RequestModel;

public class CustomerQuestRequestModel
{
    public string? BeginPoint { get; set; }
    public string? EndPoint { get; set; }
    public string? CustomerId { get; set; }
    public Guid QuestId { get; set; }
}