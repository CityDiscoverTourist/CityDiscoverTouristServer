using CityDiscoverTourist.Business.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestController : ControllerBase
{
    private readonly IQuestService _questService;

    public QuestController(IQuestService questService)
    {
        _questService = questService;
    }

    [HttpGet("{id:guid}")]
    public IActionResult Index(Guid id)
    {
        var a = _questService.Get(id).Result;
        return Ok(_questService.Get(id));
    }
}