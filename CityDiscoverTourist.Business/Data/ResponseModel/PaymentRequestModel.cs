using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class PaymentResponseModel
{
    public int Id { get; set; }
    public string? PaymentMethod { get; set; }
    public int Quantity { get; set; }
    public double AmountTotal { get; set; }
    public string? Status { get; set; }

    public CustomerQuestResponseModel CustomerQuest { get; set; }
    public int CustomerQuestId { get; set; }
}