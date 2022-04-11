namespace CityDiscoverTourist.Business.Data.RequestModel;

public class QuestRequestModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public float Price { get; set; }
    public string? EstimateTime { get; set; }
    public DateTime? AvailableTime { get; set; } = null;

    public string? Status { get; set; } = "Pending";

    public int QuestTypeId { get; set; }
}