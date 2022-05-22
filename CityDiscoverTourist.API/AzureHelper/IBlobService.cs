namespace CityDiscoverTourist.API.AzureHelper;

public interface IBlobService
{
    public Task<Stream> GetBlogAsync(string name);
    public Task<bool> UploadBlogAsync(IFormFile file);
}