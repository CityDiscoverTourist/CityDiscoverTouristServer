namespace CityDiscoverTourist.Business.Helper.Params;

public class OwnerPaymentPeriodParams : QueryStringParams
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}