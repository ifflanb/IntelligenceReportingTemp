using IntelligenceReporting.Entities;
using IntelligenceReporting.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntelligenceReporting.WebApi.Controllers;

/// <summary>Office API</summary>
[Route("api/[controller]")]
[ApiController]
public class StaticDataController : ControllerBase
{
    private readonly IStaticDataService _staticDataService;

    /// <summary>Constructor</summary>
    public StaticDataController(IStaticDataService staticDataService)
    {
        _staticDataService = staticDataService;
    }

    /// <summary>Get static data</summary>
    /// <returns>Returns <see cref="StaticData"/></returns>
    [HttpGet]
    public async Task<ActionResult<StaticData>> Get()
    {
        var results = await _staticDataService.GetStaticData();
        return Ok(results);
    }

}