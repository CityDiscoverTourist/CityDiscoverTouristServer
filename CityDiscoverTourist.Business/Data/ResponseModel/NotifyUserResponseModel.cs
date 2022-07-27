namespace CityDiscoverTourist.Business.Data.ResponseModel;

public class NotifyUserResponseModel
{
    public string? UserId { get; set; }

    public int NotifyId { get; set; }

    public bool HasRead { get; set; }
}