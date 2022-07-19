using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace CityDiscoverTourist.Business.IServices.Services;

public class ImageComparison : IImageComparison
{
    private readonly IBlobService _blobService;

    public ImageComparison(IBlobService blobService)
    {
        _blobService = blobService;
    }

    public async Task<long> CompareImages(int questItemId, List<byte[]> sceneByte)
    {
        var baseUrl = await _blobService.GetBaseUrl("quest-item", 0);
        var listBytes = new List<byte[]>();

        var client = new HttpClient();

        // Get the image base from the blob storage
        foreach (var url in baseUrl)
        {
            var imageBase = await client.GetByteArrayAsync(url);
            listBytes.Add(imageBase);
        }

        var listImageBase = ConvertImage(listBytes);

        // get the image from device to compare with the base image
        //var listImageScene = ConvertImage(sceneByte);


        var exampleImage1 =
            new Image<Gray, byte>(
                "D:\\C#\\CityDiscoverTourist\\CityDiscoverTourist.API\\bin\\Debug\\net6.0\\exampleImg\\a.jpg");
        var exampleImage2 =
            new Image<Gray, byte>(
                "D:\\C#\\CityDiscoverTourist\\CityDiscoverTourist.API\\bin\\Debug\\net6.0\\exampleImg\\b.jpg");
        var exampleImage3 =
            new Image<Gray, byte>(
                "D:\\C#\\CityDiscoverTourist\\CityDiscoverTourist.API\\bin\\Debug\\net6.0\\exampleImg\\c.jpg");
        var exampleImage4 =
            new Image<Gray, byte>(
                "D:\\C#\\CityDiscoverTourist\\CityDiscoverTourist.API\\bin\\Debug\\net6.0\\exampleImg\\d.jpg");
        var exampleImage5 =
            new Image<Gray, byte>(
                "D:\\C#\\CityDiscoverTourist\\CityDiscoverTourist.API\\bin\\Debug\\net6.0\\exampleImg\\e.jpg");
        var exampleImage6 =
            new Image<Gray, byte>(
                "D:\\C#\\CityDiscoverTourist\\CityDiscoverTourist.API\\bin\\Debug\\net6.0\\exampleImg\\f.jpg");

        var sceneImageArr = new List<Image<Gray, byte>>
        {
            exampleImage6,
            exampleImage5,
            exampleImage4
        };

        return CompareImages(listImageBase, sceneImageArr);
    }

    private static long CompareImages(List<Image<Gray, byte>> listImageBase, List<Image<Gray, byte>> listImageScene)
    {
        var sift = new SIFT();
        var bfMatcher = new BFMatcher(DistanceType.L2Sqr);

        long mostMatches = 0;

        foreach (var exampleImg in listImageBase)
        {
            var vectorOfKeypoints = new VectorOfKeyPoint();
            var originalDescriptor = new Mat();

            sift.DetectAndCompute(exampleImg, null, vectorOfKeypoints, originalDescriptor, false);

            foreach (var image in listImageScene)
            {
                var vectorOfKeypointsOfScene = new VectorOfKeyPoint();
                var descriptorsOfScene = new Mat();


                sift.DetectAndCompute(image, null, vectorOfKeypointsOfScene, descriptorsOfScene, false);

                var vectorOfMatches = new VectorOfVectorOfDMatch();

                bfMatcher.KnnMatch(descriptorsOfScene, originalDescriptor, vectorOfMatches, 2);

                //every matches alogirthm found
                var matches = vectorOfMatches.ToArrayOfArray();

                //matches with distance less than 0.75
                var goodMatches = matches.Where(x => x[0].Distance < 0.75 * x[1].Distance).ToArray();

                //number of good matches
                var numberOfGoodMatches = goodMatches.Length;

                //if number of good matches is greater than most matches, then set most matches to number of good matches
                if (numberOfGoodMatches > mostMatches) mostMatches = numberOfGoodMatches;
            }
        }

        return mostMatches;
    }

    private static List<Image<Gray, byte>> ConvertImage(List<byte[]> image)
    {
        var listImage = new List<Image<Gray, byte>>();
        foreach (var item in image)
        {
            var mat = new Mat();
            CvInvoke.Imdecode(item, ImreadModes.Grayscale, mat);
            var exampleImage = mat.ToImage<Gray, byte>();
            listImage.Add(exampleImage);
        }
        return listImage;
    }
}