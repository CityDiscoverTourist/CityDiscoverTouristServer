namespace CityDiscoverTourist.Business.Data.RequestModel;

public class AreaRequestModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }

    public int CityId { get; set; }
}