using Microsoft.AspNetCore.Http;

namespace CityDiscoverTourist.Business.Data.RequestModel;

public class QuestTypeRequestModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }
    public string? DurationMode { get; set; }
    public string? DistanceMode { get; set; }
    public IFormFile? Image { get; set; }

    public void Validate()
    {
        Name = Name?.Trim();
        Status = Status?.Trim();
    }
}