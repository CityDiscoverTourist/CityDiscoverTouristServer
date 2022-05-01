namespace CityDiscoverTourist.Business.Data.RequestModel;

public class QuestRequestModel
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public float Price { get; set; }
    public string? EstimatedTime { get; set; }
    public string? EstimatedDistance { get; set; }
    public DateTime? AvailableTime { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? Status { get; set; } = "Pending";
    public int QuestTypeId { get; set; }
    public int? QuestOwnerId { get; set; }
}