namespace CityDiscoverTourist.Business.Data.RequestModel;

public class QuestItemRequestModel
{
    public int Id { get; set; }
    public int QuestItemTypeId { get; set; }

    public int LocationId { get; set; }

    public int QuestId { get; set; }

    public string? Content { get; set; }
    public string? Description { get; set; }
    public int Duration { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? QrCode { get; set; }
    public int TriggerMode { get; set; }
    public string? RightAnswer { get; set; }
    public string? AnswerImageUrl { get; set; }
    public string? Status { get; set; }

    public int? ItemId { get; set; }

    public void Validate()
    {
        Content = Content?.Trim();
        Description = Description?.Trim();
        QrCode = QrCode?.Trim();
        RightAnswer = RightAnswer?.Trim();
        AnswerImageUrl = AnswerImageUrl?.Trim();
        Status = Status?.Trim();
    }
}