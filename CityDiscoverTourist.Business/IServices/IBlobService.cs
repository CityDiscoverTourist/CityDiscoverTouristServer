using Microsoft.AspNetCore.Http;

namespace CityDiscoverTourist.Business.IServices;

public interface IBlobService
{
    public Task<Stream> GetBlogAsync(string name);
    public Task<bool> UploadBlogAsync(IFormFile file, string containerName);
    public Task<string> UploadQuestImgAndReturnImgPathAsync(IFormFile? file, int questId, string containerName);
    public Task<string> GetImgPathAsync(string name);
}