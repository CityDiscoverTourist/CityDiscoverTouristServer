namespace CityDiscoverTourist.Business.IServices.Services;

public class BaseService
{
    protected static void CheckDataNotNull(string name, object value)
    {
        throw value switch
        {
            Task => new InvalidOperationException("Should not pass task here"),
            null => new KeyNotFoundException($"{name} not found"),
            _ => new ArgumentException("Exception occured")
        };
    }
}