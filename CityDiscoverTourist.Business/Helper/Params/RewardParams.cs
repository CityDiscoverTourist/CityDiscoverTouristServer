namespace CityDiscoverTourist.Business.Helper.Params;

public class RewardParams : QueryStringParams
{
    public string? Name { get; set; }
    public DateTime? ExpiredDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public string? CustomerEmail { get; set; }
}