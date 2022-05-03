namespace CityDiscoverTourist.Business.Data.RequestModel;

public class RewardRequestModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime? ExpiredDate { get; set; }
    public DateTime ReceivedDate { get; set; }

    public string? CustomerId { get; set; }
    public int QuestId { get; set; }

    public string? Status { get; set; }
}