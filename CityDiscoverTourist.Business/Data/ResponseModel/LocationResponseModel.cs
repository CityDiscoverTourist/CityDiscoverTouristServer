namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class LocationResponseModel
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

    public List<QuestItemResponseModel>? QuestItems { get; set; }
}