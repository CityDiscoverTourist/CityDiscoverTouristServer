using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class Answer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? RightAnswer { get; set; }
    public string? Status { get; set; }

    public Task? Task { get; set; }
    public int TaskId { get; set; }
}