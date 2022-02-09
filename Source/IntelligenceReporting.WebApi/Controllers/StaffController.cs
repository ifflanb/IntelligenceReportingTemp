using IntelligenceReporting.Queries;
using IntelligenceReporting.Searchers;
using Microsoft.AspNetCore.Mvc;

namespace IntelligenceReporting.WebApi.Controllers;

/// <summary>Staff API</summary>
[Route("api/[controller]")]
[ApiController]
public class StaffController : ControllerBase
{
    private readonly IStaffSearcher _searcher;

    /// <summary>Constructor</summary>
    public StaffController(IStaffSearcher searcher)
    {
        _searcher = searcher;
    }

    /// <summary>Query staff members</summary>
    /// <returns>Returns <see cref="StaffPagedResults"/></returns>
    [HttpGet]
    public async Task<ActionResult<StaffPagedResults>> Get([FromQuery] StaffQueryParameters parameters)
    {
        var results = await _searcher.Query(parameters);
        return Ok(results);
    }

}