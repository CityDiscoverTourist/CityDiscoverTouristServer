namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class RewardResponseModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime? ExpiredDate { get; set; }
    public DateTime ReceivedDate { get; set; }

    public string? CustomerId { get; set; }
    public int QuestId { get; set; }

    public string? Status { get; set; }
}