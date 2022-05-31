using CityDiscoverTourist.Business.Helper;

namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class QuestResponseModel
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public float Price { get; set; }
    public string? ImagePath { get; set; }
    public string? EstimatedTime { get; set; }
    public string? EstimatedDistance { get; set; }
    public string? AvailableTime { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? Status { get; set; }
    public int QuestTypeId { get; set; }
    public int? QuestOwnerId { get; set; }
    public int AreaId { get; set; }
}