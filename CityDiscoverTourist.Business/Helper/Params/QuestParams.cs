namespace CityDiscoverTourist.Business.Helper.Params;

public class QuestParams : QueryStringParams
{
    public string? Status { get; set; }
    public string? Name { get; set; }
    public string? Description { get ; set ; }
    public int QuestTypeId { get; set; }
}