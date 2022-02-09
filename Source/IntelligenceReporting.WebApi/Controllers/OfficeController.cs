using IntelligenceReporting.Queries;
using IntelligenceReporting.Searchers;
using Microsoft.AspNetCore.Mvc;

namespace IntelligenceReporting.WebApi.Controllers;

/// <summary>Office API</summary>
[Route("api/[controller]")]
[ApiController]
public class OfficeController : ControllerBase
{
    private readonly IOfficeSearcher _searcher;

    /// <summary>Constructor</summary>
    public OfficeController(IOfficeSearcher searcher)
    {
        _searcher = searcher;
    }

    /// <summary>Query offices</summary>
    /// <returns>Returns an <see cref="OfficePagedResults"/></returns>
    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> Get([FromQuery] OfficeQueryParameters parameters)
    {
        var results = await _searcher.Query(parameters);
        return Ok(results);
    }

}