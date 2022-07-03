namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class AreaResponseModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }

    public int CityId { get; set; }

    public List<LocationResponseModel> Locations { get; set; }
}