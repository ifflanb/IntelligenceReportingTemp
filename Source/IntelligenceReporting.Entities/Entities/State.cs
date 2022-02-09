namespace IntelligenceReporting.Entities;

/// <summary>The states</summary>
public class State: SynchronizedEntity
{
    /// <summary> State name eg. New South Wales</summary>
    public string StateName { get; set; } = "";
    /// <summary> State abbreviation eg. NSW</summary>
    public string StateAbbreviation { get; set; } = "";
    /// <summary> The country id this state belongs to </summary>
    public int CountryId { get; set; }
    /// <summary> Time zone standard names eg. AUS Eastern Standard Time</summary>
    public string TimeZoneName { get; set; }
}