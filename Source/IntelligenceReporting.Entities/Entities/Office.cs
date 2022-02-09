namespace IntelligenceReporting.Entities;

/// <summary>Office</summary>
public class Office: SynchronizedEntity
{
    /// <summary>The modify date time of this office</summary>
    /// <remarks>This is used to sync with the source database</remarks>
    public DateTimeOffset LastUpdated { get; set; }

    /// <summary>The name of the office</summary>
    public string OfficeName { get; set; } = "";

    /// <summary>The brand of this office (e.g. Harcourts, Ray White)</summary>
    public int? BrandId { get; set; }

    /// <summary>The state this office is in (e.g. Victoria)</summary>
    public int? StateId { get; set; }

    /// <summary>The multi-office this office belongs to (e.g. Cooper &amp; Co, Grenadier)</summary>
    public int? MultiOfficeId { get; set; }

    /// <summary>This office is active</summary>
    public bool IsActive { get; set; }
}