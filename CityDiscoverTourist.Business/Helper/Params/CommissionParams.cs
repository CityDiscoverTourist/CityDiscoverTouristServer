namespace CityDiscoverTourist.Business.Helper.Params;

public class CommissionParams : QueryStringParams
{
    public int MinAmount { get; set; }
    public int MaxAmount { get; set; }
    public int MinCount { get; set; }
    public int MaxCount { get; set; }
    public int Percentage { get; set; }

    public int QuestTypeId { get; set; }
}