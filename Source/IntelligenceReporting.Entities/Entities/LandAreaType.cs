namespace IntelligenceReporting.Entities;

/// <summary>Land area type (unit of area)</summary>
public class LandAreaType: Entity
{
    /// <summary>Name of the land area type</summary>
    public string LandAreaTypeName { get; set; } = "";
}


/// <summary>Land area type ids</summary>
public enum LandAreaTypeId
{
    /// <summary>Square meter</summary>
    SquareMeters = 1,
    /// <summary>Acre</summary>
    Acre = 2,
    /// <summary>Hectare</summary>
    Hectare = 3
}