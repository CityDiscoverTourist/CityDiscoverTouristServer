namespace CityDiscoverTourist.API.AzureHelper;

public class BlobInfo
{
    public Stream Stream { get; set; }
    public string? ContentType { get; set; }

    public BlobInfo(Stream stream, string? contentType)
    {
        Stream = stream;
        ContentType = contentType;
    }
}