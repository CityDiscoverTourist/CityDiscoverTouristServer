namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class LocationTypeResponseModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }

    public List<LocationResponseModel>? Locations { get; set; }
}