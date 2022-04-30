namespace CityDiscoverTourist.Data.Models;

public class Competition: BaseEntity
{
    public Quest? Quest { get; set; }
    public int QuestId { get; set; }

    public string? CompetitionCode { get; set; }

    public List<CustomerQuest>? CustomerQuests { get; set; }
}