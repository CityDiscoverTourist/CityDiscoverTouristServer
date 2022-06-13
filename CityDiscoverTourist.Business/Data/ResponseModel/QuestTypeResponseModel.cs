namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class QuestTypeResponseModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }
    public string? DurationMode { get; set; }
    public string? DistanceMode { get; set; }
    public string? ImagePath { get; set; }
    public List<QuestResponseModel>? Quests { get; set; }
}