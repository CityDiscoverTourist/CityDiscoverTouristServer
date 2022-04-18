namespace CityDiscoverTourist.Business.Data.RequestModel;

public class TaskRequestModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }
    public int Duration { get; set; }
    public int Experience { get; set; }
    public string? UrlStory { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int TaskTypeId { get; set; }

    public Guid QuestId { get; set; }

    public string? Status { get; set; }
}