using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace CityDiscoverTourist.Business.IServices.Services;

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

    public async Task<bool> UploadBlogAsync(IFormFile file, string containerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(file.FileName);
        await blobClient.UploadAsync(file.OpenReadStream(), true);
        return true;
    }

    // upload file to blob storage with pattern name = {id}_{fileName}
    public async Task<string> UploadQuestImgAndReturnImgPathAsync(IFormFile? file, int questId, string containerName)
    {
        if (file == null) return null;

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient($"{questId}_{file.FileName}");
        await blobClient.UploadAsync(file.OpenReadStream(), true);
        return blobClient.Uri.AbsoluteUri;
    }


    // get image from blob storage
    public  Task<string> GetImgPathAsync(string name)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("quest");
        var blobClient = containerClient.GetBlobClient(name);
        return Task.FromResult(blobClient.Uri.AbsoluteUri);
    }
}