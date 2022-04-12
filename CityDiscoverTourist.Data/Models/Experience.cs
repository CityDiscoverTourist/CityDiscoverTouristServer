using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class Experience: BaseEntity
{
    public int TotalScore { get; set; }

    public ApplicationUser Customer { get; set; }
    public string? CustomerId { get; set; }

    public string? Status { get; set; }
}