namespace CityDiscoverTourist.Data.Models;

public class CustomerAnswer: BaseEntity
{
    public string? Note { get; set; }
    public string? CustomerReply { get; set; }

    public CustomerTask? CustomerTask { get; set; }
    public int CustomerTaskId { get; set; }

    public QuestItem? QuestItem { get; set; }
    public int QuestItemId { get; set; }

    public string? AnswerImageUrl { get; set; }
}