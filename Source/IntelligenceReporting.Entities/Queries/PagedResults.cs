using IntelligenceReporting.Entities;

namespace IntelligenceReporting.Queries
{
    /// <summary>Paged query results</summary>
    /// <typeparam name="TParameters">The type of the parameters for the query requested</typeparam>
    /// <typeparam name="TResult">The query results</typeparam>
    public class PagedResults<TParameters, TResult> where TParameters: QueryParameters, new()
    {
        /// <summary>The parameters for the query requested</summary>
        public TParameters Parameters { get; set; }

        /// <summary>The total count of results available</summary>
        public int TotalCount { get; set; }

        /// <summary>The results for this page</summary>
        public TResult[] Results { get; set; }


        /// <summary>Create an empty paged results</summary>
        public PagedResults()
        {
            Parameters = new TParameters();
            TotalCount = 0;
            Results = Array.Empty<TResult>();
        }

    }


    /// <summary>Paged multi-office query results</summary>
    public class MultiOfficePagedResults : PagedResults<MultiOfficeQueryParameters, MultiOffice> { }


    /// <summary>Paged office query results</summary>
    public class OfficePagedResults : PagedResults<OfficeQueryParameters, OfficeQueryResult> { }


    /// <summary>Paged staff query results</summary>
    public class StaffPagedResults : PagedResults<StaffQueryParameters, StaffQueryResult> { }
}
