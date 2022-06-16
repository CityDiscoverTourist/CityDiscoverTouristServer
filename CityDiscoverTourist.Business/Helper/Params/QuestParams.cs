namespace CityDiscoverTourist.Business.Helper.Params;

public class QuestParams : QueryStringParams
{
    public string? Name { get; set; }
    public string? Description { get ; set ; }
    public int QuestTypeId { get; set; }
    public void Validate()
    {
        Name = Name?.Trim();
        Status = Status?.Trim();
        Description = Description?.Trim();
    }
}