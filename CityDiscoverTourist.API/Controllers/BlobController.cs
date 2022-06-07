using CityDiscoverTourist.API.AzureHelper;
using CityDiscoverTourist.Business.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class BlobController : ControllerBase
{
    private readonly IBlobService _blobService;

    public BlobController(IBlobService blobService)
    {
        _blobService = blobService;
    }

    [HttpGet("nganluong_10d04f903b41a83e948f5653627e1a22.html")]
    public async Task<IActionResult> GetFileAsync()
    {
        var result = await _blobService.GetImgPathAsync("nganluong_10d04f903b41a83e948f5653627e1a22.html");
        return base.Content(result, "text/html");
    }

    [HttpPost]
    public async Task<IActionResult> UploadAsync(IFormFile file)
    {
        var result = await _blobService.UploadBlogAsync(file, "");
        return Ok(result);
    }
}