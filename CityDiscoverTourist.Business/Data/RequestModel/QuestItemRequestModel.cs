using Microsoft.AspNetCore.Http;

namespace CityDiscoverTourist.Business.Data.RequestModel;

public class QuestItemRequestModel
{
    public int Id { get; set; }
    public int QuestItemTypeId { get; set; }
    public IFormFile[]? Image { get; set; }
    public IFormFile? ImageDescription { get; set; }
    public string? PathImageDescription { get; set; }
    public List<string>? ListImages { get; set; }
    public int LocationId { get; set; }
    public int QuestId { get; set; }
    public string? Content { get; set; }
    public string? Story { get; set; }
    public string? Description { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? RightAnswer { get; set; }
    public string? Status { get; set; }
    public int? ItemId { get; set; }

    public void Validate()
    {
        Content = Content?.Trim();
        Description = Description?.Trim();
        RightAnswer = RightAnswer?.Trim();
        Status = Status?.Trim();
    }
}