using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using CityDiscoverTourist.Business.Exceptions;

namespace CityDiscoverTourist.Business.IServices.Services;

public class BlobService : BaseService, IBlobService
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

    public async Task<string> UploadAvatarImgPathAsync(IFormFile? file, string customerId, string containerName)
    {
        if (file == null) return null!;

        var renameFile = file.FileName.Replace(file.FileName, containerName);

        // create container if not exist
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient($"{customerId}_{renameFile}");
        await blobClient.UploadAsync(file.OpenReadStream(), true);

        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<string> UploadQuestItemImgAsync(IFormFile?[]? file, int questItemId, string containerName)
    {
        var imageUrl = "";
        if (file == null) return null!;


        for (var i = 0; i < file.Length; i++)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(questItemId + "/" + file[i]!.FileName);

            await blobClient.UploadAsync(file[i]!.OpenReadStream(), true);

            // get the url not get the file name
            imageUrl = blobClient.Uri.ToString().Replace(file[i]!.FileName, "");
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

    //delete file from blob storage
    public async Task<bool> DeleteBlogAsync(string name, string containerName, int questItemId)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(questItemId + "/" + name);
        var a = await blobClient.DeleteIfExistsAsync();
        return true;
    }
}