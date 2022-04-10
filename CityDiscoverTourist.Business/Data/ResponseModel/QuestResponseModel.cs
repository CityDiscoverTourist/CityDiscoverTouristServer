namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class QuestResponseModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public float Price { get; set; }
    public string? EstimateTime { get; set; }
    public DateTime? AvailableTime { get; set; } = null;

    public DateTime? CreatedDate { get; set; }
    public string? Status { get; set; }

    public int QuestTypeId { get; set; }

}