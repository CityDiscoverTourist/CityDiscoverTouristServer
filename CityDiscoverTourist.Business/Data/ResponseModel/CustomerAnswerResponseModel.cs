namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class CustomerAnswerResponseModel
{
    public int Id { get; set; }
    public int CustomerTaskId { get; set; }

    public int QuestItemId { get; set; }

    public string? AnswerImageUrl { get; set; }
}