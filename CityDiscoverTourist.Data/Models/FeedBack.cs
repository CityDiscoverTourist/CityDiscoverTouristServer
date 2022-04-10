using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class FeedBack
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Content { get; set; }
    public int Rating { get; set; }
    public ApplicationUser Customer { get; set; }
    public string? CustomerId { get; set; }

    public Quest Quest { get; set; }
    public Guid? QuestId { get; set; }
}