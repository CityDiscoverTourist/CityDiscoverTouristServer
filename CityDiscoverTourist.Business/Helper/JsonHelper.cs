namespace CityDiscoverTourist.Business.Helper;

public static class JsonHelper
{
    public static string JsonFormat(string? obj)
    {
        // format received is vietnamese string()english string
        var result = obj!.Split("()");
        return @"{
	                'vi': '" + result[0].Trim() + @"',
	                'en': '" + result[1].Trim() + @"'
                }";
    }
}