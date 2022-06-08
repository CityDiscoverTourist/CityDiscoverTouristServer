namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class CustomerAnswerResponseModel
{
    public string? Note { get; set; }
    public string? CustomerReply { get; set; }
    public int Id { get; set; }
    public int CustomerTaskId { get; set; }

    public int QuestItemId { get; set; }

    public string? AnswerImageUrl { get; set; }
}