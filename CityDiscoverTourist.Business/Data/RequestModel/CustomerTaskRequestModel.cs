namespace CityDiscoverTourist.Business.Data.RequestModel;

public class CustomerTaskRequestModel
{
    public float CurrentPoint { get; set; }
    public string? Status { get; set; }
    public int TaskId { get; set; }
    public string? ApplicationUserId { get; set; }
}