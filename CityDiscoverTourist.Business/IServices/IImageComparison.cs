namespace CityDiscoverTourist.Business.IServices;

public interface IImageComparison
{
    public Task<long> CompareImages(int questItemId, List<byte[]> image);
}