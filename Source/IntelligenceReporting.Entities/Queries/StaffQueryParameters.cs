namespace IntelligenceReporting.Queries
{
    /// <summary>Staff query parameters</summary>
    public class StaffQueryParameters : QueryParameters
    {
        /// <summary>Restrict results to an office</summary>
        public int? OfficeId { get; set; }

        /// <summary>Restrict to staff containing this name</summary>
        public string? StaffName { get; set; }

        /// <summary>Restrict to this brand</summary>
        public int? BrandId { get; set; }

        /// <summary>Restrict to this country</summary>
        public int? CountryId { get; set; }

        /// <summary>Restrict to this state</summary>
        public int? StateId { get; set; }

        /// <summary>Restrict to this multi-office group (franchise)</summary>
        public int? MultiOfficeId { get; set; }

        /// <summary>Restrict to active staff</summary>
        public bool? IsActive { get; set; } = true;
    }
}
