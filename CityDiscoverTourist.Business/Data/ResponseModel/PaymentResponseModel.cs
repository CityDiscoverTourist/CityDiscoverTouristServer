namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class PaymentResponseModel
{
    public Guid Id { get; set; }
    public string? PaymentMethod { get; set; }
    public int Quantity { get; set; }
    public double TotalAmount { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? CustomerId { get; set; }
    public string? CustomerEmail { get; set; }
    public bool IsValid { get; set; }
    public int QuestId { get; set; }
    public string? QuestName { get; set; }
    public string? QuestDescription { get; set; }
    public string? ImagePath { get; set; }
}