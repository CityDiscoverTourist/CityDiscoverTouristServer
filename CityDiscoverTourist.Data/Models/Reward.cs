using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class Reward: BaseEntity
{
    public string? Name { get; set; }
    public DateTime ReceivedDate { get; set; }
    public DateTime? ExpiredDate { get; set; }

    public ApplicationUser Customer { get; set; }
    public string CustomerId { get; set; }

    public string? Status { get; set; }

    public Reward()
    {
        ReceivedDate = DateTime.UtcNow.Date;
    }
}