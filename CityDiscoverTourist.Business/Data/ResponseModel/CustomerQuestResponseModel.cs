using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class CustomerQuestResponseModel
{
    public string? BeginPoint { get; set; }
    public string? EndPoint { get; set; }
    public string? CustomerId { get; set; }
    public Guid QuestId { get; set; }
    public Payment? PaymentMethod { get; set; }
}