namespace CityDiscoverTourist.Data.Models;

public class QuestItemType: BaseEntity
{
    public string? Name { get; set; }
    public string? Status { get; set; }

    public List<QuestItem>? QuestItems { get; set; }
}