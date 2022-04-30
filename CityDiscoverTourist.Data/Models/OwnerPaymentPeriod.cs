namespace CityDiscoverTourist.Data.Models;

public class OwnerPaymentPeriod: BaseEntity
{
    public DateTime? CreatedDate { get; set; }
    public DateTime? EndDate { get; set; }

    public List<OwnerPayment>? OwnerPayments { get; set; }
}