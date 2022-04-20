namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class CustomerTaskResponseModel
{
    public float CurrentPoint { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedDate { get; set; } = null;
    public int TaskId { get; set; }
    public string? CustomerId { get; set; }
}