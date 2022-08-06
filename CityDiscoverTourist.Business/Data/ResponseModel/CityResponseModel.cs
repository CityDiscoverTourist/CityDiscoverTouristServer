namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class CityResponseModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public List<AreaResponseModel>? Areas { get; set; }
}