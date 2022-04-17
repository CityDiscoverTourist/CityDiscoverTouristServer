namespace CityDiscoverTourist.Data.Models;

public class CustomerTask: BaseEntity
{
    public float CurrentPoint { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public List<CustomerAnswer>? CustomerAnswers { get; set; }
    public List<Note> Notes { get; set; }
    public Task? Task { get; set; }
    public int TaskId { get; set; }

}