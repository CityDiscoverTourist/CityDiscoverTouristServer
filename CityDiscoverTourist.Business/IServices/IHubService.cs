namespace CityDiscoverTourist.Business.IServices;

public interface IHubService
{
    public Task GetMessage(string message);
}