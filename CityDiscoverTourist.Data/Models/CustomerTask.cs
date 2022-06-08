namespace CityDiscoverTourist.Data.Models;

public class CustomerTask: BaseEntity
{
    public float CurrentPoint { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int CountSuggestion { get; set; }
    public bool IsFinished { get; set; }
    public int CountWrongAnswer { get; set; }

    public QuestItem? QuestItem { get; set; }
    public int QuestItemId { get; set; }

    public CustomerQuest? CustomerQuest { get; set; }
    public int CustomerQuestId { get; set; }

    public List<CustomerAnswer>? CustomerAnswers { get; set; }
    public List<Note>? Notes { get; set; }
}