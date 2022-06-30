using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class PaymentResponseModel
{
    public Guid Id { get; set; }
    public string? PaymentMethod { get; set; }
    public int Quantity { get; set; }
    public double AmountTotal { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? CustomerId { get; set; }

    public int QuestId { get; set; }
}