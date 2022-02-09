using System.ComponentModel;

namespace IntelligenceReporting.Queries;

/// <summary>Basic query parameters</summary>
public class QueryParameters
{
    /// <summary>The number of results to return per page</summary>
    [DefaultValue(200)]
    public int PageSize { get; set; } = 200;

    /// <summary>The page to return (1-based)</summary>
    [DefaultValue(1)]
    public int Page { get; set; } = 1;

    /// <summary>The sort order of the results (property names, suffix with " d" or " descending" for descending order)</summary>
    [DefaultValue("")]
    public string OrderBy { get; set; } = "";
}
