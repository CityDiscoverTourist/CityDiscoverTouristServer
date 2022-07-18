using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace CityDiscoverTourist.Business.IServices.Services;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<Stream> GetBlogAsync(string name)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("item");
        var blobClient = containerClient.GetBlobClient("example/" + name);
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
        if (file == null) return null!;

        var renameFile = file.FileName.Replace(file.FileName, containerName);

        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient($"{questId}_{renameFile}");
        await blobClient.UploadAsync(file.OpenReadStream(), true);

        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<string> UploadQuestItemImgAsync(IFormFile?[]? file, int questItemId, string containerName)
    {
        var imageUrl = "";
        if (file == null) return null!;

        for (var i = 0; i < file.Length; i++)
        {
            var renameFile = file[i]!.FileName.Replace(file[i]!.FileName, containerName);

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(questItemId + "/" + renameFile + i);
            await blobClient.UploadAsync(file[i]!.OpenReadStream(), true);

            imageUrl = blobClient.Uri.AbsoluteUri.Split(renameFile + i)[0];
        }

        return imageUrl!;
    }

    // get all list images url from blob storage
    public async Task<List<string>> GetBaseUrl(string containerName, int questItemId)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(questItemId + "/");

        var blobInfo = blobClient.Name;

        var baseUrl = containerClient.Uri.AbsoluteUri + "/";

        var list = new List<string>();

        await foreach (var blob in containerClient.GetBlobsAsync(prefix: blobInfo))
        {
            list.Add(baseUrl + blob.Name);
        }
        return list;
    }
}