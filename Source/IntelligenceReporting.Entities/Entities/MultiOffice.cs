namespace IntelligenceReporting.Entities
{
    /// <summary>A Multi-Office grouping (Franchise)</summary>
    public class MultiOffice : SynchronizedEntity
    {
        /// <summary>The name of the Multi-Office grouping (e.g. Cooper &amp; Co, Grenadier)</summary>
        public string MultiOfficeName { get; set; } = "";
    }
}
