namespace CityDiscoverTourist.Business.Data.RequestModel;

public class LocationRequestModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public string? Address { get; set; }
    public string? Status { get; set; }

    public int AreaId { get; set; }

    public int LocationTypeId { get; set; }

    public void Validate()
    {
        Name = Name?.Trim();
        Description = Description?.Trim();
        Longitude = Longitude?.Trim();
        Latitude = Latitude?.Trim();
        Address = Address?.Trim();
        Status = Status?.Trim();
    }
}