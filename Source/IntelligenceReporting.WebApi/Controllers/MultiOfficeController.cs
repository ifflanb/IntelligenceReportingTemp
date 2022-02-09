using IntelligenceReporting.Entities;
using IntelligenceReporting.Queries;
using IntelligenceReporting.Searchers;
using Microsoft.AspNetCore.Mvc;

namespace IntelligenceReporting.WebApi.Controllers;

/// <summary>Multi-Office API</summary>
[Route("api/[controller]")]
[ApiController]
public class MultiOfficeController : ControllerBase
{
    private readonly IMultiOfficeSearcher _searcher;

    /// <summary>Constructor</summary>
    public MultiOfficeController(IMultiOfficeSearcher searcher)
    {
        _searcher = searcher;
    }

    /// <summary>Query multi-offices</summary>
    /// <returns>Returns a list of <see cref="MultiOffice"/></returns>
    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> Get([FromQuery] MultiOfficeQueryParameters parameters)
    {
        var results = await _searcher.Query(parameters);
        return Ok(results);
    }

}