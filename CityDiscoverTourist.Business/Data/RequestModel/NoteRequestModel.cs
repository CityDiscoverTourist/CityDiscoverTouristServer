namespace CityDiscoverTourist.Business.Data.RequestModel;

public class NoteRequestModel
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public string? Image { get; set; }

    public int CustomerTaskId { get; set; }
}