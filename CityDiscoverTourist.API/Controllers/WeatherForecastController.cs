using CityDiscoverTourist.Business.IServices;
using Diacritics.Extensions;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly IBlobService   _blobService;
    private readonly IImageComparison _imageComparison;

    public WeatherForecastController(IBlobService blobService,
        IImageComparison imageComparison)
    {
        _blobService = blobService;
        _imageComparison = imageComparison;
    }

    [HttpGet(Name = "test")]
    public async Task<long> Get()
    {
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

        var exampleArray = new List<Image<Gray, byte>>
        {
            exampleImage1,
            exampleImage2,
            exampleImage3,
            exampleImage4,
            exampleImage5,
            exampleImage6
        };
        var s = await _imageComparison.CompareImages(0, null);
        //object in scene
        //Image<Gray, Byte> sceneImage1 = new Image<Gray, Byte>("C:\\\\Users\\\\khang\\\\source\\\\repos\\\\EmguDemo\\\\EmguDemo\\\\bin\\\\Debug\\\\net6.0\\\\scene.jpg");
        var sceneImage2 =
            new Image<Gray, byte>(
                "D:\\C#\\CityDiscoverTourist\\CityDiscoverTourist.API\\bin\\Debug\\net6.0\\exampleImg\\a.jpg");
        var sceneImage1 =
            new Image<Gray, byte>(
                "D:\\C#\\CityDiscoverTourist\\CityDiscoverTourist.API\\bin\\Debug\\net6.0\\exampleImg\\b.jpg");
        var sceneImage3 =
            new Image<Gray, byte>(
                "D:\\C#\\CityDiscoverTourist\\CityDiscoverTourist.API\\bin\\Debug\\net6.0\\exampleImg\\f.jpg");

        var sceneImageArr = new List<Image<Gray, byte>>
        {
            sceneImage1,
            sceneImage2,
            sceneImage3
        };

        var sift = new SIFT();
        var bfMatcher = new BFMatcher(DistanceType.L2Sqr);

        long mostMatches = 0;

        foreach (var exampleImg in exampleArray)
        {
            var vectorOfKeypoints = new VectorOfKeyPoint();
            var originalDescriptor = new Mat();

            sift.DetectAndCompute(exampleImg, null, vectorOfKeypoints, originalDescriptor, false);

            foreach (var image in sceneImageArr)
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


    [HttpGet("demo")]
    public async Task<long> Demo()
    {
        var s = await _blobService.GetBaseUrl("quest-item", 1);
        var baseUrl = s.ToString();
        //load image from url
        var client = new HttpClient();
        var image1 = client.GetAsync("https://citytouriststorage.blob.core.windows.net/quest-item/0/quest-item0").Result
            .Content.ReadAsByteArrayAsync().Result;
        var image2 = client.GetAsync("https://citytouriststorage.blob.core.windows.net/quest-item/0/quest-item1").Result
            .Content.ReadAsByteArrayAsync().Result;
        var image3 = client.GetAsync("https://citytouriststorage.blob.core.windows.net/quest-item/0/quest-item2").Result
            .Content.ReadAsByteArrayAsync().Result;
        var image4 = client.GetAsync("https://citytouriststorage.blob.core.windows.net/quest-item/0/quest-item3").Result
            .Content.ReadAsByteArrayAsync().Result;
        var image5 = client.GetAsync("https://citytouriststorage.blob.core.windows.net/quest-item/0/quest-item4").Result
            .Content.ReadAsByteArrayAsync().Result;
        var image6 = client.GetAsync("https://citytouriststorage.blob.core.windows.net/quest-item/0/quest-item0").Result
            .Content.ReadAsByteArrayAsync().Result;


        var exampleImage1 = ConvertImage(image1);

        var exampleImage2 = ConvertImage(image2);

        var exampleImage3 = ConvertImage(image3);

        var exampleImage4 = ConvertImage(image4);


        // convert byte array to bitmap
        /*var bitmap1 = new Bitmap(new MemoryStream(image1));
        var bitmap2 = new Bitmap(new MemoryStream(image2));
        var bitmap3 = new Bitmap(new MemoryStream(image3));
        var bitmap4 = new Bitmap(new MemoryStream(image4));
        var bitmap5 = new Bitmap(new MemoryStream(image5));
        var bitmap6 = new Bitmap(new MemoryStream(image6));

        //example object
        var exampleImage1 = bitmap1.ToImage<Gray, byte>();
        var exampleImage2 = bitmap2.ToImage<Gray, byte>();
        var exampleImage3 = bitmap3.ToImage<Gray, byte>();
        var exampleImage4 = bitmap4.ToImage<Gray, byte>();
        var exampleImage5 = bitmap5.ToImage<Gray, byte>();
        var exampleImage6 = bitmap6.ToImage<Gray, byte>();*/

        var exampleArray = new List<Image<Gray, byte>>
        {
            exampleImage1,
            exampleImage2,
            exampleImage3,
            exampleImage4
        };

        //object in scene

        var sceneImageArray = new List<Image<Gray, byte>>
        {
            exampleImage1,
            exampleImage4
        };

        var sift = new SIFT();
        var bfMatcher = new BFMatcher(DistanceType.L2Sqr);

        long mostMatches = 0;

        foreach (IInputArray exampleImg in exampleArray)
        {
            var vectorOfKeypoints = new VectorOfKeyPoint();
            var originalDescriptor = new Mat();

            sift.DetectAndCompute(exampleImg, null, vectorOfKeypoints, originalDescriptor, false);

            foreach (IInputArray image in sceneImageArray)
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

    [HttpPost("demo2")]
    public Task<long> Demo2([FromForm] List<IFormFile> file)
    {
        return _imageComparison.CompareImages(110, file);
    }

    private static Image<Gray, byte> ConvertImage(byte[] image1)
    {
        var mat = new Mat();
        CvInvoke.Imdecode(image1, ImreadModes.Grayscale, mat);
        var exampleImage = mat.ToImage<Gray, byte>();
        return exampleImage;
    }


    /*//total good matches has > 75% accuracy
    long goodMatches = 0;
    //define accuracy
    const float accuracy = 0.75f;

    foreach(var match in matches)
    {
        if (match[0].Distance < accuracy * match[1].Distance)
        {
            goodMatches++;
        }
    }

    //update mostMatches
    if (goodMatches > mostMatches)
    {
        mostMatches = goodMatches;
    }*/
}