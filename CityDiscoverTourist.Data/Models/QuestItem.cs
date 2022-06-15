using System.ComponentModel;

namespace CityDiscoverTourist.Data.Models;

public class QuestItem: BaseEntity
{
    public QuestItemType? QuestTypeItem { get; set; }
    public int QuestItemTypeId { get; set; }

    public Location? Location { get; set; }
    public int LocationId { get; set; }

    public Quest? Quest { get; set; }
    public int QuestId { get; set; }

    public string? Content { get; set; }
    public string? Description { get; set; }
    public int Duration { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? QrCode { get; set; }
    public string? RightAnswer { get; set; }
    public string? AnswerImageUrl { get; set; }

    [DefaultValue(null)] public QuestItem? Item { get; set; } = null;
    public int? ItemId { get; set; }

    public string? Status { get; set; }

    public List<Suggestion>? Suggestions { get; set; }
    public List<CustomerAnswer>? CustomerAnswers { get; set; }
}