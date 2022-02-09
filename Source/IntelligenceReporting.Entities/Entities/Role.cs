namespace IntelligenceReporting.Entities;

/// <summary>Agent roles</summary>
public class Role: Entity
{
    /// <summary>Name of the role</summary>
    public string RoleName { get; set; } = "";
}


/// <summary>Agent role ids</summary>
public enum RoleId
{
    /// <summary>Lister</summary>
    Lister = 1,
    /// <summary>Seller</summary>
    Seller = 2,
    /// <summary>Referrer</summary>
    Referrer = 7,
    /// <summary>Other</summary>
    Other = 8
}