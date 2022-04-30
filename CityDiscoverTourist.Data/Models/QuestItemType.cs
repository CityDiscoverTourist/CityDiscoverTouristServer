namespace CityDiscoverTourist.Data.Models;

public class QuestItemType: BaseEntity
{
    public string? Name { get; set; }
    public int Status { get; set; }

    public List<QuestItem>? QuestItems { get; set; }
}