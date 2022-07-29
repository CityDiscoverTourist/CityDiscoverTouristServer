namespace CityDiscoverTourist.Business.Helper.Params;

public abstract class QueryStringParams
{
    private const int MaxPageSize = 1000;
    private int _pageSize = 10;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string? OrderBy { get; set; }
    public string? Status { get; set; }
}