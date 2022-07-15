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


    // get image from blob storage
    public async Task<string> GetImgPathAsync(string name)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("quest");
        var blobClient = containerClient.GetBlobClient(name);
        //var blobInfo = await blobClient.DownloadAsync();
        var line = "";
        if (!await blobClient.ExistsAsync()) return line;

        var response = await blobClient.DownloadAsync();

        using var streamReader = new StreamReader(response.Value.Content);
        while (!streamReader.EndOfStream)
        {
            line = await streamReader.ReadLineAsync();
            Console.WriteLine(line);
        }

        return line!;
    }

    // get all images from blob storage
    public async Task<List<string>> GetAllImgPathAsync(string containerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient("example/");

        var list = new List<string>();

        await  foreach (var blobItem in containerClient.GetBlobsAsync()) list.Add(blobItem.Name);

        return list;
    }
}