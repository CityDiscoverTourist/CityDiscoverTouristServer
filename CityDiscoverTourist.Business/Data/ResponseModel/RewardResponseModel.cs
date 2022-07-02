namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class RewardResponseModel
{
    public string? Name { get; set; }
    public DateTime? ExpiredDate { get; set; }

    public string? CustomerId { get; set; }

    public int QuestRewardId { get; set; }

    public string? Status { get; set; }
}