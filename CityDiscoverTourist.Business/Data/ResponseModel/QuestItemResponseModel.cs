namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class QuestItemResponseModel
{
    public int Id { get; set; }
    public int QuestItemTypeId { get; set; }

    public int LocationId { get; set; }

    public int QuestId { get; set; }

    public string? Content { get; set; }
    public string? Description { get; set; }
    public int Duration { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? QrCode { get; set; }
    public string? RightAnswer { get; set; }
    public string AnswerImageUrl { get; set; }
    public string? Status { get; set; }
    public List<string>? ListImages { get; set; }
    public List<SuggestionResponseModel>? Suggestions { get; set; }
    public int ItemId { get; set; }
}