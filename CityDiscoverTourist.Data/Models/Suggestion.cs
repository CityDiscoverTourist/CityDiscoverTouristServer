using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class Suggestion: BaseEntity
{
    public string? Content { get; set; }

    public Task? Task { get; set; }
    public int TaskId { get; set; }

    public string? Status { get; set; }
}