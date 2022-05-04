namespace CityDiscoverTourist.Business.Helper.Params;

public class RewardParams : QueryStringParams
{
    public string? Name { get; set; }
    public DateTime? ExpiredDate { get; set; }
    public DateTime? ReceivedDate{ get; set; }
    public int CustomerId { get; set; }
    public int QuestId { get; set; }
}