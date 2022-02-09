namespace IntelligenceReporting.Queries;

/// <summary>Paged agent earnings query results</summary>
public class AgentEarningsPagedResults : PagedResults<AgentEarningsQueryParameters, AgentEarningsQueryResult>
{
    /// <summary>The totals across all results</summary>
    public AgentEarningsQueryResult Total { get; set; } = new();

    /// <summary>The average across all results</summary>
    public AgentEarningsQueryResult Average { get; set; } = new();
}