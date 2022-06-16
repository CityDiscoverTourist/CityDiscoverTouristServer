namespace CityDiscoverTourist.Business.Helper.Params;

public class AreaParams : QueryStringParams
{
    public int CityId { get; set; }
    public string? Name { get; set; }

    public void Validate()
    {
        Name = Name?.Trim();
        Status = Status?.Trim();
    }
}