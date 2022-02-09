namespace IntelligenceReporting.Entities;

/// <summary>The country</summary>
public class Country: SynchronizedEntity
{
    /// <summary> The name of country </summary>
    public string CountryName { get; set; } = "";

    /// <summary> The ISO code of country </summary>
    public string IsoCountryCode { get; set; } = "";
}