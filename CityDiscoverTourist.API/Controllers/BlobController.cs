using CityDiscoverTourist.API.AzureHelper;
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

    [HttpGet]
    public async Task<IActionResult> GetFileAsync()
    {
        var result = await _blobService.GetBlogAsync("student.PNG");
        return File(result, "application/octet-stream");
    }

    [HttpPost]
    public async Task<IActionResult> UploadAsync(IFormFile file)
    {
        var result = await _blobService.UploadBlogAsync(file);
        return Ok(result);
    }
}