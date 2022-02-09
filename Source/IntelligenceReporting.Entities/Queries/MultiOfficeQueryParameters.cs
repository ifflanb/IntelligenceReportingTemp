namespace IntelligenceReporting.Queries;

/// <summary>Multi-office query parameters</summary>
public class MultiOfficeQueryParameters : QueryParameters
{
    /// <summary>Restrict to multi-office groups containing this name</summary>
    public string? Name { get; set; }
}