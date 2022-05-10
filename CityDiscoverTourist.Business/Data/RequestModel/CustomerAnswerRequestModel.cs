namespace CityDiscoverTourist.Business.Data.RequestModel;

public class CustomerAnswerRequestModel
{
    public int Id { get; set; }
    public int CustomerTaskId { get; set; }

    public int QuestItemId { get; set; }

    public string? AnswerImageUrl { get; set; }
}