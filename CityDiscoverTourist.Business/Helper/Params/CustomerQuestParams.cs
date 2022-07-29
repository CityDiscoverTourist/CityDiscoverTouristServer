namespace CityDiscoverTourist.Business.Helper.Params;

public class CustomerQuestParams : QueryStringParams
{
    public int QuestId { get; set; }
    public string? CustomerEmail { get; set; }
    public bool? IsFinished { get ; set ; } = null;
    public bool? IsFeedbackApproved { get; set; }
}