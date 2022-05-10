namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class CommissionResponseModel
{
    public int Id { get; set; }
    public int MinAmount { get; set; }
    public int MaxAmount { get; set; }
    public int MinCount { get; set; }
    public int MaxCount { get; set; }
    public int Percentage { get; set; }

    public int QuestTypeId { get; set; }
}