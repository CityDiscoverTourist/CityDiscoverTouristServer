namespace CityDiscoverTourist.Business.Helper.Params;

public class TaskParams: QueryStringParams
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
}