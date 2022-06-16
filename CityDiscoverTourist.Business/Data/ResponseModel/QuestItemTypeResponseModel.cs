namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class QuestItemTypeResponseModel
{
    public int Id { get; set; }

    public string? Name { get; set; }
    public string? Status { get; set; }
    public List<QuestItemResponseModel>? QuestItems { get; set; }
}