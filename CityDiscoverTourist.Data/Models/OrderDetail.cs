using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityDiscoverTourist.Data.Models;

public class OrderDetail
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int Quantity { get; set; }
    public string? PaymentMethod { get; set; }
    public string? DiscountPercent { get; set; }
    public string? Status { get; set; }

    public Order Order { get; set; }
    public int OrderId { get; set; }
}