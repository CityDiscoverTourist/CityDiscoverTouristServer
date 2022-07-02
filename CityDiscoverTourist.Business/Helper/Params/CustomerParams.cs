namespace CityDiscoverTourist.Business.Helper.Params;

public class CustomerParams : QueryStringParams
{
    public string? Email { get; set; }
    public bool? IsLock { get; set; } = null;
}