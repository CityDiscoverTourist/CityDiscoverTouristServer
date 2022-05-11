using CityDiscoverTourist.Data.Models;

namespace CityDiscoverTourist.Business.Data.RequestModel;

public class CustomerQuestRequestModel
{
    public int Id { get; set; }
    public string? BeginPoint { get; set; }
    public string? EndPoint { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int Rating { get; set; }
    public string? FeedBack { get; set; }

    public string? CustomerId { get; set; }
    public int CompetitionId { get; set; }
}