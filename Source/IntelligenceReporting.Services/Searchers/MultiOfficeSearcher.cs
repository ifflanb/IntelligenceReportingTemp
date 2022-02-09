using IntelligenceReporting.Databases;
using IntelligenceReporting.Entities;
using IntelligenceReporting.Queries;

namespace IntelligenceReporting.Searchers;

public class MultiOfficeSearcher : Searcher<MultiOfficeQueryParameters, MultiOffice, MultiOfficePagedResults>, IMultiOfficeSearcher
{
    public MultiOfficeSearcher(IntelligenceReportingDbContext dbContext) : base(dbContext) { }

    protected override IQueryable<MultiOffice> GetQuery(MultiOfficeQueryParameters parameters)
    {
        IQueryable<MultiOffice> query = Context.Set<MultiOffice>();

        if (!string.IsNullOrEmpty(parameters.Name))
            query = query.Where(x => x.MultiOfficeName.Contains(parameters.Name));

        return query;
    }

    protected override IOrderedQueryable<MultiOffice> GetDefaultOrderBy(IQueryable<MultiOffice> query)
        => query.OrderBy(o => o.MultiOfficeName);
}
