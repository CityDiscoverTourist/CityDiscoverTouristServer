namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class TaskResponseModel
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }
    public int Duration { get; set; }
    public int TaskTypeId { get; set; }
    public Guid QuestId { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedDate { get; set; }
}