using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class Answer: BaseEntity
{
    public string? RightAnswer { get; set; }
    public string? Status { get; set; }

    public Task? Task { get; set; }
    public int TaskId { get; set; }
}