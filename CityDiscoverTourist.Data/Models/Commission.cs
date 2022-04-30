namespace CityDiscoverTourist.Data.Models;

public class Commission: BaseEntity
{
    public int MinAmount { get; set; }
    public int MaxAmount { get; set; }
    public int MinCount { get; set; }
    public int MaxCount { get; set; }
    public int Percentage { get; set; }

    public QuestType? QuestType { get; set; }
    public int QuestTypeId { get; set; }
}