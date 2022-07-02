namespace CityDiscoverTourist.Business.Data.RequestModel;

public class CustomerTaskRequestModel
{
    public int Id { get; set; }
    public string? Status { get; set; }

    public int CustomerQuestId { get; set; }
}