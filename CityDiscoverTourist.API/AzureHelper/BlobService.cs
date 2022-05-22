using Azure.Storage.Blobs;

namespace CityDiscoverTourist.API.AzureHelper;

public class BlobService: IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<Stream> GetBlogAsync(string name)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("quest");
        var blobClient = containerClient.GetBlobClient(name);
        var blobInfo = await blobClient.DownloadAsync();
        return blobInfo.Value.Content;
    }

    public async Task<bool> UploadBlogAsync(IFormFile file)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("quest");
        var blobClient = containerClient.GetBlobClient("UserId_" + file.FileName);
        await blobClient.UploadAsync(file.OpenReadStream(), true);
        return true;
    }
}