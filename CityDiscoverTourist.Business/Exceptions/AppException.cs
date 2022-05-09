using System.Globalization;

namespace CityDiscoverTourist.Business.Exceptions;

public class AppException: System.Exception
{
    public AppException() : base() {}

    public AppException(string message) : base(message){ }
    public AppException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}