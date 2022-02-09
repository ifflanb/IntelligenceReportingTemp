using System.ComponentModel.DataAnnotations;

namespace IntelligenceReporting.Queries;

/// <summary>An office query result</summary>
public class OfficeQueryResult
{
    /// <summary>The id of the office</summary>
    [Key]
    public int OfficeId { get; set; }
    
    /// <summary>The name of the office</summary>
    public string OfficeName { get; set; } = "";

    /// <summary>Id of brand</summary>
    public int? BrandId { get; set; }

    /// <summary>Name of the brand (e.g. Harcourts, Ray White)</summary>
    public string? BrandName { get; set; } = "";

    /// <summary>Id of the country</summary>
    public int? CountryId { get; set; }

    /// <summary>Id of the state</summary>
    public int? StateId { get; set; }

    /// <summary>Name of the state (e.g. Victoria)</summary>
    public string? StateName { get; set; } = "";

    /// <summary>The id of the multi-office this office belongs to</summary>
    public int? MultiOfficeId { get; set; }

    /// <summary>The name of the multi-office this office belongs to (e.g. Cooper &amp; Co, Grenadier)</summary>
    public string MultiOfficeName { get; set; } = "";

    /// <summary>Office is active (not demo or locked)</summary>
    public bool IsActive { get; set; }
}