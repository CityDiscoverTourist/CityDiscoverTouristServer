using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class Experience
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int TotalScore { get; set; }

    public ApplicationUser Customer { get; set; }
    public string? CustomerId { get; set; }

    public string? Status { get; set; }
}