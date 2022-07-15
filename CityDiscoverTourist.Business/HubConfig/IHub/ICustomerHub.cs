namespace CityDiscoverTourist.Business.HubConfig.IHub;

public interface ICustomerHub
{
    Task NewCustomerCreated(string email);
}