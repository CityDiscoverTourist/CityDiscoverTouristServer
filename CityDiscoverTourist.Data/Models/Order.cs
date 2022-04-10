using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public float Price { get; set; }
    public int DiscountPrice { get; set; }
    public int OrderDetailId { get; set; }
    public DateTime OrderDate { get; set; }

    public ApplicationUser Customer { get; set; }
    public string? CustomerId { get; set; }

    public List<OrderDetail>? OrderDetails { get; set; }

    public string? Status { get; set; }
}