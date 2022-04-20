namespace CityDiscoverTourist.Data.Models;

public class CustomerAnswer: BaseEntity
{
    public string? Answer { get; set; }
    public string? Status { get; set; }

    public CustomerTask CustomerTask { get; set; }
    public int CustomerTaskId { get; set; }
}