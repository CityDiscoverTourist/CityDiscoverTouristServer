namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class RewardResponseModel
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public DateTime? ExpiredDate { get; set; }
    public int PercentDiscount { get; set; }

    public string? CustomerId { get; set; }
    public string? CustomerEmail { get; set; }

    public string? Status { get; set; }
}