using Microsoft.AspNetCore.Http;

namespace CityDiscoverTourist.Business.Data.RequestModel;

public class QuestRequestModel
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public float Price { get; set; }
    public string? EstimatedTime { get; set; }
    public string? EstimatedDistance { get; set; }
    public IFormFile? Image { get; set; }
    public string? AvailableTime { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? Status { get; set; } = "Pending";
    public int QuestTypeId { get; set; }
    public int AreaId { get; set; }

    public void Validate()
    {
        Title = Title?.Trim();
        Description = Description?.Trim();
        EstimatedTime = EstimatedTime?.Trim();
        EstimatedDistance = EstimatedDistance?.Trim();
        AvailableTime = AvailableTime?.Trim();
        Status = Status?.Trim();
    }
}