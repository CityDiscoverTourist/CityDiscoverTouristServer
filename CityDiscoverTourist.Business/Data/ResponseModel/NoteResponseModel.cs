namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class NoteResponseModel
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public string? Image { get; set; }

    public int CustomerTaskId { get; set; }
}