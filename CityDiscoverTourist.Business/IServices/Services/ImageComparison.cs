using System.Net.Mime;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.AspNetCore.Http;

namespace CityDiscoverTourist.Business.IServices.Services;

public class ImageComparison : IImageComparison
{
    private readonly IBlobService _blobService;

    public ImageComparison(IBlobService blobService)
    {
        _blobService = blobService;
    }

    public async Task<long> CompareImages(int questItemId, List<IFormFile> sceneImage)
    {
        var baseUrl = await _blobService.GetBaseUrl("quest-item", questItemId);
        var listBytes = new List<byte[]>();

        var client = new HttpClient();

        // Get the image base from the blob storage
        foreach (var url in baseUrl)
        {
            var imageBase = await client.GetByteArrayAsync(url);
            listBytes.Add(imageBase);
        }

        var listImageBase = ConvertImage(listBytes);

        //get the image from device to compare with the base image
        var listByteScene = ConvertImageFromUser(sceneImage);
        var listImageScene = ConvertImage(listByteScene);

        return CompareImages(listImageBase, listImageScene);
    }

    async Task<bool> IImageComparison.CompareImage(int questItemId, List<IFormFile> image)
    {
        var baseUrl = await _blobService.GetBaseUrl("quest-item", questItemId);
        var listBytes = new List<byte[]>();

        var client = new HttpClient();

        // Get the image base from the blob storage
        foreach (var url in baseUrl)
        {
            var imageBase = await client.GetByteArrayAsync(url);
            listBytes.Add(imageBase);
        }

        var listImageBase = ConvertImage(listBytes);

        //get the image from device to compare with the base image
        var listByteScene = ConvertImageFromUser(image);
        var listImageScene = ConvertImage(listByteScene);

        return CompareImages(listImageBase, listImageScene) > 5000;
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
            //re-size the image to match the size of the base image
            var mat = new Mat(100, 200, DepthType.Cv8U, 0);
            CvInvoke.Imdecode(item, ImreadModes.Grayscale, mat);
            var exampleImage = mat.ToImage<Gray, byte>();
            listImage.Add(exampleImage);
        }
        return listImage;
    }

    // convert image from file to byte array
    private static List<byte[]> ConvertImageFromUser(List<IFormFile> file)
    {
        var listImage = new List<byte[]>();
        foreach (var item in file)
        {
            //re-size image to fit the model
            var image = item.OpenReadStream();
            var bytes = new byte[image.Length];
            image.Read(bytes, 0, bytes.Length);
            listImage.Add(bytes);
        }
        return listImage;
    }
}