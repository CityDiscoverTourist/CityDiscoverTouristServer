namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class SuggestionResponseModel
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public string? Status { get; set; }

    public int QuestItemId { get; set; }
}