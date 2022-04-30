namespace CityDiscoverTourist.Data.Models;

public class CustomerAnswer: BaseEntity
{
    public CustomerTask? CustomerTask { get; set; }
    public int CustomerTaskId { get; set; }

    public QuestItem? QuestItem { get; set; }
    public int QuestItemId { get; set; }

    public string? AnswerImageUrl { get; set; }
}