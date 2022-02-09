namespace IntelligenceReporting.Entities;

/// <summary>The source of data e.g. VaultRE</summary>
public class Source : Entity
{
    /// <summary>Name of the source database</summary>
    public string SourceName { get; set; } = "";
}


/// <summary>Source database ids</summary>
public enum SourceId
{
    /// <summary>VaultRE database</summary>
    VaultRe = 1,
}