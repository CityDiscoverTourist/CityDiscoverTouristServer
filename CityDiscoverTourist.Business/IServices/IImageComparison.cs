using Microsoft.AspNetCore.Http;

namespace CityDiscoverTourist.Business.IServices;

public interface IImageComparison
{
    public Task<long> CompareImages(int questItemId, List<IFormFile> image);
}