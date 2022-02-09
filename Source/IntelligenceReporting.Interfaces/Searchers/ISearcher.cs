using IntelligenceReporting.Entities;
using IntelligenceReporting.Queries;

namespace IntelligenceReporting.Searchers;

public interface ISearcher<in TParameters, TResult, TPagedResults> 
    where TParameters : QueryParameters, new()
    where TPagedResults : PagedResults<TParameters, TResult>, new()
{
    Task<TPagedResults> Query(TParameters parameters);
}


public interface IAgentEarningsSearcher : ISearcher<AgentEarningsQueryParameters, AgentEarningsQueryResult, AgentEarningsPagedResults> { }
public interface IOfficeSearcher : ISearcher<OfficeQueryParameters, OfficeQueryResult, OfficePagedResults> { }
public interface IMultiOfficeSearcher : ISearcher<MultiOfficeQueryParameters, MultiOffice, MultiOfficePagedResults> { }
public interface IStaffSearcher : ISearcher<StaffQueryParameters, StaffQueryResult, StaffPagedResults> { }
