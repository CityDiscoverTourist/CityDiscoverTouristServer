namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class QuestRewardResponseModel
{
    public Guid Code { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? Status { get; set; }
    public int PercentDiscount { get; set; }
    public int PercentPointRemain { get; set; }

    public int QuestId { get; set; }
}