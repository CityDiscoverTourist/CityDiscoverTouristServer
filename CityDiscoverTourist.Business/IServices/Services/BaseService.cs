using CityDiscoverTourist.Business.Enums;
using Newtonsoft.Json.Linq;

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

    protected static string ConvertLanguage(Language language, string? text)
    {
        return JObject.Parse(text)[language.ToString()]!.ToString();
    }

    protected static DateTime CurrentDateTime()
    {
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
    }

    protected static string Trim(string requestName)
    {
        return requestName.Replace(" ", string.Empty).Trim();
    }

    protected static string GetVietNameseName(string requestName)
    {
        return requestName.Split("()")[0];
    }

    // check file is image or not
    protected static bool IsImage(string fileName)
    {
            return fileName.EndsWith(".jpg") || fileName.EndsWith(".png") || fileName.EndsWith(".jpeg") || fileName.EndsWith(".gif")
               || fileName.EndsWith(".bmp") || fileName.EndsWith(".tiff");
    }
}