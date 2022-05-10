namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class PaymentResponseModel
{
    public int Id { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Status { get; set; }

    public int CustomerQuestId { get; set; }
}