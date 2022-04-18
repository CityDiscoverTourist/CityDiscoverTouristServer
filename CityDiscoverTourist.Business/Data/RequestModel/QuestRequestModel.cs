namespace CityDiscoverTourist.Business.Data.RequestModel;

public class QuestRequestModel
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public float Price { get; set; }
    public string? EstimatedTime { get; set; }
    public string? EstimatedDistance { get; set; }
    public DateTime? AvailableTime { get; set; } = null;

    public DateTime? UpdatedDate { get; set; } = null;
    public string? Status { get; set; } = "Pending";
    public int QuestTypeId { get; set; }
}