namespace CityDiscoverTourist.Business.Data.RequestModel;

public class LocationTypeRequestModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }

    public void Validate()
    {
        Name = Name?.Trim();
        Status = Status?.Trim();
    }
}