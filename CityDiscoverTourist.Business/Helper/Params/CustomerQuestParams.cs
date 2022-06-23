namespace CityDiscoverTourist.Business.Helper.Params;

public class CustomerQuestParams : QueryStringParams
{
    public int QuestId { get; set; }
    public string? CustomerEmail { get; set; }
}