namespace CityDiscoverTourist.Business.Helper.Params;

public class QuestParams : QueryStringParams
{
    public string? Name { get; set; }
    public string? Description { get ; set ; }
    public int QuestTypeId { get; set; }
    public int AreaId { get; set; }
}