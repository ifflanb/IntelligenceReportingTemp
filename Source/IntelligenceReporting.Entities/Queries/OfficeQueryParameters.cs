namespace IntelligenceReporting.Queries;

/// <summary>Office query parameters</summary>
public class OfficeQueryParameters : QueryParameters
{
    /// <summary>Restrict to offices containing this name</summary>
    public string? OfficeName { get; set; }

    /// <summary>Restrict to offices with this brand (e.g. Harcourts, Ray White)</summary>
    public int? BrandId { get; set; }

    /// <summary>Restrict to this country</summary>
    public int? CountryId { get; set; }

    /// <summary>Restrict to offices belonging to this state (e.g. Victoria)</summary>
    public int? StateId { get; set; }

    /// <summary>Restrict to offices belonging to this multi-office group (e.g. Cooper &amp; Co, Grenadier)</summary>
    public int? MultiOfficeId { get; set; }

    /// <summary>Restrict to active offices. Default value is true</summary>
    public bool? IsActive { get; set; } = true;
}