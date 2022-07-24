using Microsoft.AspNetCore.Http;

namespace CityDiscoverTourist.Business.IServices;

public interface IBlobService
{
    public Task<Stream> GetBlogAsync(string name);
    public Task<bool> UploadBlogAsync(IFormFile file, string containerName);
    public Task<string> UploadQuestImgAndReturnImgPathAsync(IFormFile? file, int questId, string containerName);
    public Task<string> UploadAvatarImgPathAsync(IFormFile? file, string customerId, string containerName);
    public Task<string> UploadQuestItemImgAsync(IFormFile?[]? file, int questItemId, string containerName);
    public Task<List<string>> GetBaseUrl(string containerName, int questItemId);
    public Task<bool> DeleteBlogAsync(string name, string containerName, int questItemId);
}