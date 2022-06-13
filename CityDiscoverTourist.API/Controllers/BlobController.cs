using CityDiscoverTourist.Business.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

/// <summary>
/// </summary>
[Route("api/v{version:apiVersion}/[controller]s")]
[ApiVersion("1.0")]
[ApiController]
public class BlobController : ControllerBase
{
    private readonly IBlobService _blobService;

    /// <summary>
    /// </summary>
    /// <param name="blobService"></param>
    public BlobController(IBlobService blobService)
    {
        _blobService = blobService;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    [HttpGet("nganluong_10d04f903b41a83e948f5653627e1a22.html")]
    public async Task<IActionResult> GetFileAsync()
    {
        var result = await _blobService.GetImgPathAsync("nganluong_10d04f903b41a83e948f5653627e1a22.html");
        return base.Content(result, "text/html");
    }

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> UploadAsync(IFormFile file)
    {
        var result = await _blobService.UploadBlogAsync(file, "");
        return Ok(result);
    }
}