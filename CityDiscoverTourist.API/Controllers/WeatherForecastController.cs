using System.Drawing;
using CityDiscoverTourist.Business.Helper.EmailHelper;
using CityDiscoverTourist.Business.IServices;
using Diacritics.Extensions;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

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
    private readonly IEmailSender _emailSender;

    public WeatherForecastController(IBlobService blobService,
        IImageComparison imageComparison, IEmailSender emailSender)
    {
        _blobService = blobService;
        _imageComparison = imageComparison;
        _emailSender = emailSender;
    }

    [HttpGet]
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
        /*var exampleImage4 =
            new Image<Gray, byte>(
                "D:\\C#\\CityDiscoverTourist\\CityDiscoverTourist.API\\bin\\Debug\\net6.0\\exampleImg\\d.jpg");
        var exampleImage5 =
            new Image<Gray, byte>(
                "D:\\C#\\CityDiscoverTourist\\CityDiscoverTourist.API\\bin\\Debug\\net6.0\\exampleImg\\e.jpg");
        var exampleImage6 =
            new Image<Gray, byte>(
                "D:\\C#\\CityDiscoverTourist\\CityDiscoverTourist.API\\bin\\Debug\\net6.0\\exampleImg\\f.jpg");*/

        var exampleArray = new List<Image<Gray, byte>>
        {
            exampleImage1,
            exampleImage2,
            exampleImage3,
        };
        //var s = await _imageComparison.CompareImages(0, null);
        //object in scene
        //Image<Gray, Byte> sceneImage1 = new Image<Gray, Byte>("C:\\\\Users\\\\khang\\\\source\\\\repos\\\\EmguDemo\\\\EmguDemo\\\\bin\\\\Debug\\\\net6.0\\\\scene.jpg");
        var sceneImage2 =
            new Image<Gray, byte>(
                "D:\\C#\\CityDiscoverTourist\\CityDiscoverTourist.API\\bin\\Debug\\net6.0\\exampleImg\\q.jpg");
        var sceneImage1 =
            new Image<Gray, byte>(
                "D:\\C#\\CityDiscoverTourist\\CityDiscoverTourist.API\\bin\\Debug\\net6.0\\exampleImg\\w.jpg");
        var sceneImage3 =
            new Image<Gray, byte>(
                "D:\\C#\\CityDiscoverTourist\\CityDiscoverTourist.API\\bin\\Debug\\net6.0\\exampleImg\\e.jpg");

        var sceneImageArr = new List<Image<Gray, byte>>
        {
            sceneImage1,
            //sceneImage2,
            //sceneImage3
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




    [HttpPost("demo2")]
    public Task<long> Demo2([FromForm] List<IFormFile> file)
    {
        var stream = new MemoryStream();
        using var ms = stream;
        QRCodeGenerator qRCodeGenerator = new();
        var data = qRCodeGenerator.CreateQrCode("input", QRCodeGenerator.ECCLevel.L);
        QRCode qRCode = new(data);
        using var bitmap = qRCode.GetGraphic(20);
        var a = data.GetRawData(QRCodeData.Compression.Deflate);
        var w = Convert.ToBase64String(a);

        var message = "<h1>Payment Success</h1>" + "<h3>Dear " +   "</h3>" +
                      "<p>Your payment has been succeeded</p>" + "<p>Your order is: " +  "</p>" +
                      "<p>Your quest name is: " + "/ " +
                      "</p>" + "<p>Quantity is: " +
                      "</p>" + "<p>Your order total amount is: " +  "</p>" +
                      "<img src=\"data:image/jpeg;base64, " + w + "\"" + "alt=\"Text\""  + "/>";

        _emailSender.SendMailConfirmAsync("datlqse140263@fpt.edu.vn", "ok", message);
        return _imageComparison.CompareImages(110, file);
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