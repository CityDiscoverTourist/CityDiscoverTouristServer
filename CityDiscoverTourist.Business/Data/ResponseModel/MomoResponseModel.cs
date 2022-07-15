namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class MomoResponseModel
{
    public string? Amount { get; set; }
    public string? Message { get; set; }
    public Guid OrderId { get; set; }
    public string? PartnerCode { get; set; }
    public string? RequestId { get; set; }
    public string? ResponseTime { get; set; }
    public string? RequestType { get; set; }
    public string? ResultCode { get; set; }
    public string? Signature { get; set; }
    public string? TransId { get; set; }
}