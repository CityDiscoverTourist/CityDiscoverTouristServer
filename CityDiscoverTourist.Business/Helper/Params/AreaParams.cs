namespace CityDiscoverTourist.Business.Helper.Params;

public class AreaParams: QueryStringParams
{
    public int CityId { get; set; }
    public int QuestId { get; set; }
}