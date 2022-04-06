using System.ComponentModel.DataAnnotations;

namespace CityDiscoverTourist.Data.Models;

public class Quest
{
    [Key]
    public Guid Id { get; set; }
}