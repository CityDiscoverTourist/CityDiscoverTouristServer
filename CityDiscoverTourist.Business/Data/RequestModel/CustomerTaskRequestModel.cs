namespace CityDiscoverTourist.Business.Data.RequestModel;

public class CustomerTaskRequestModel
{
    public int Id { get; set; }
    public float CurrentPoint { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedDate { get; set; }

    public int QuestItemId { get; set; }

    public int CustomerQuestId { get; set; }
}