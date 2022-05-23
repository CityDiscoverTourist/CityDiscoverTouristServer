using Microsoft.AspNetCore.Http;

namespace CityDiscoverTourist.Business.IServices;

public interface IBlobService
{
    public Task<Stream> GetBlogAsync(string name);
    public Task<bool> UploadBlogAsync(IFormFile file);
    public Task<string> UploadQuestImgAndReturnImgPathAsync(IFormFile? file, int questId);
    public Task<string> GetImgPathAsync(string name);
}