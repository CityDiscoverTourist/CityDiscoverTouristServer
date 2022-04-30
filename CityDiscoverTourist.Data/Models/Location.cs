using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class Location: BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
    public string? Address { get; set; }
    public string? Status { get; set; }

    public int AreaId { get; set; }
    public Area? Area { get; set; }

    public LocationType? LocationType { get; set; }
    public int LocationTypeId { get; set; }

    public List<QuestItem>? QuestItems { get; set; }
}