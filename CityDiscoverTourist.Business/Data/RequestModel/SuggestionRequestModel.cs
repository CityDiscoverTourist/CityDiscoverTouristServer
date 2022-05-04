namespace CityDiscoverTourist.Business.Data.RequestModel;

public class SuggestionRequestModel
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public string? Status { get; set; }

    public int QuestItemId { get; set; }
}