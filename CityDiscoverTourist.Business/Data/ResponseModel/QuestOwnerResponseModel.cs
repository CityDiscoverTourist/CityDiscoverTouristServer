namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class QuestOwnerResponseModel
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? Status { get; set; }

    public int WalletId { get; set; }
}