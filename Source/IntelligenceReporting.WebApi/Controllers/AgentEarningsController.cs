using IntelligenceReporting.Queries;
using IntelligenceReporting.Searchers;
using Microsoft.AspNetCore.Mvc;

namespace IntelligenceReporting.WebApi.Controllers;

/// <summary>Agent Earnings API</summary>
[Route("api/[controller]")]
[ApiController]
public class AgentEarningsController : ControllerBase
{
    private readonly IAgentEarningsSearcher _searcher;

    /// <summary>Constructor</summary>
    public AgentEarningsController(IAgentEarningsSearcher searcher)
    {
        _searcher = searcher;
    }

    /// <summary>Query agent earnings</summary>
    /// <returns>Returns <see cref="AgentEarningsPagedResults"/></returns>
    [HttpGet]
    public async Task<ActionResult<AgentEarningsPagedResults>> Get([FromQuery] AgentEarningsQueryParameters parameters)
    {
        var results = await _searcher.Query(parameters);
        return Ok(results);
    }

}