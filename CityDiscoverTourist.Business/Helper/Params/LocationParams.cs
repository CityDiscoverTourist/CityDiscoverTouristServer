namespace CityDiscoverTourist.Business.Helper.Params;

public class LocationParams : QueryStringParams
{
    public string? Name { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public string? Address { get; set; }

    public int AreaId { get; set; }

    public int LocationTypeId { get; set; }
}