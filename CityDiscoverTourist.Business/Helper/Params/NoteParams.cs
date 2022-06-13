namespace CityDiscoverTourist.Business.Helper.Params;

public class NoteParams : QueryStringParams
{
    public string? Content { get; set; }
    public int CustomerTaskId { get; set; }
}