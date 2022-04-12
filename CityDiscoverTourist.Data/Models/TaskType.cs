using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class TaskType: BaseEntity
{
    public string? Name { get; set; }
    public string? Status { get; set; }

    public List<Task>? Tasks { get; set; }
}