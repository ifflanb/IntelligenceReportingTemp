namespace IntelligenceReporting.Entities
{
    /// <summary>A listing/appraisal agent</summary>
    public class SaleLifeAgent : Entity
    {
        /// <summary>The office</summary>
        public int OfficeId { get; set; }

        /// <summary>The staff member</summary>
        public int StaffId { get; set; }

        /// <summary>Is this the primary agent</summary>
        public bool IsPrimary { get; set; }
    }
}
