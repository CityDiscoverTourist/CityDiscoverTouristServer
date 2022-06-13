namespace CityDiscoverTourist.Business.Helper.Params;

public class TaskParams : QueryStringParams
{
    public string? Name { get; set; }
    public int QuestId { get; set; }
    public int QuestItemTypeId { get; set; }
}