namespace CityDiscoverTourist.Business.Data.RequestModel;

public class QuestRewardRequestModel
{
    public Guid Code { get; set; }
    public string? Status { get; set; }
    public int PercentDiscount { get; set; }
    public int PercentPointRemain { get; set; }

    public int QuestId { get; set; }
}