namespace CityDiscoverTourist.Business.Helper.Params;

public class QuestTypeParams : QueryStringParams
{
    public string? Name { get; set; }
    public void Validate()
    {
        Name = Name?.Trim();
        Status = Status?.Trim();
    }
}