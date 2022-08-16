using System.Drawing.Imaging;
using CityDiscoverTourist.Business.Helper.EmailHelper;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace CityDiscoverTourist.API.Controllers;

[ApiController]
[Route("[controller]")]
public class Demo : ControllerBase
{
    private readonly IEmailSender _emailSender;

    public Demo(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    [HttpPost]
    public IActionResult CreateQr(string input)
    {
        var stream = new MemoryStream();
        using var ms = stream;
        QRCodeGenerator qRCodeGenerator = new();
        var data = qRCodeGenerator.CreateQrCode(input, QRCodeGenerator.ECCLevel.L);
        QRCode qRCode = new(data);

        using var bitmap = qRCode.GetGraphic(20);
        bitmap.Save(ms, ImageFormat.Png);
        return File(ms.GetBuffer(), "image/png");
    }
}