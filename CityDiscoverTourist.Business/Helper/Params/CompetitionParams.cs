namespace CityDiscoverTourist.Business.Helper.Params;

public class CompetitionParams : QueryStringParams
{
    public string? CompetitionCode { get; set; }
    public int QuestId { get; set; }
}