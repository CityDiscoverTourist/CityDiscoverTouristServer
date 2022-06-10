
namespace CityDiscoverTourist.Business.IServices.Services;

public class BaseService
{
    protected static void CheckDataNotNull(string name, object value)
    {
        switch (value)
        {
            case Task:
                throw new InvalidOperationException("Should not pass task here");
            //throw new RequestException(ErrorCodes.DataIsEmpty, $"{name} must be not null");
            case null:
                throw new KeyNotFoundException($"{name} must be not null");
        }
    }
}