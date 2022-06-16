namespace CityDiscoverTourist.Business.Helper.Params;

public class CustomerParams : QueryStringParams
{
    public string? Email { get; set; }
    public string? IsLock { get; set; }
    public void Validate()
    {
        Email = Email?.Trim();
        Status = Status?.Trim();
    }
}