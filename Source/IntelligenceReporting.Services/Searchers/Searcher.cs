using System.Linq.Expressions;
using IntelligenceReporting.Databases;
using IntelligenceReporting.Queries;
using Microsoft.EntityFrameworkCore;

namespace IntelligenceReporting.Searchers;

public abstract class Searcher<TParameters, TResult, TPagedResults>
    where TParameters : QueryParameters, new()
    where TPagedResults: PagedResults<TParameters, TResult>, new()
{
    protected IntelligenceReportingDbContext Context { get; }

    protected Searcher(IntelligenceReportingDbContext dbContext)
    {
        Context = dbContext;
    }

    public async Task<TPagedResults> Query(TParameters parameters)
    {
        var results = GetQuery(parameters);

        IOrderedQueryable<TResult>? orderedResults = null;
        if (parameters.OrderBy != "")
        {
            var entityType = typeof(TResult);
            var genericGetPropertyGetter = typeof(GenericsSupport).GetMethod(nameof(GenericsSupport.GetPropertyGetter));
            if (genericGetPropertyGetter == null) 
                throw new InvalidOperationException($"Could not find method {nameof(GenericsSupport)}.{nameof(GenericsSupport.GetPropertyGetter)}");
            foreach (var order in parameters.OrderBy.Split(',').Select(s => s.Trim()))
            {
                var lastSpace = order.LastIndexOf(' ');
                var propertyName = lastSpace == -1 ? order : order[..lastSpace];
                var isDescending = lastSpace > -1 && new[] { 'D', 'd' }.Contains(order[lastSpace + 1]);

                var property = entityType.GetProperties().SingleOrDefault(p => string.Compare(p.Name, propertyName, StringComparison.OrdinalIgnoreCase) == 0);
                if (property == null) throw new InvalidOperationException($"Property {entityType.Name}.{propertyName} not found");

                var orderByMethodName = (orderedResults == null ? "OrderBy" : "ThenBy") + (isDescending ? "Descending" : "");
                var genericOrderByMethod = typeof(Queryable).GetMethods().Single(mi => mi.Name == orderByMethodName && mi.GetParameters().Length == 2);
                var orderByMethod = genericOrderByMethod.MakeGenericMethod(typeof(TResult), property.PropertyType);

                var thisGetPropertyGetter = genericGetPropertyGetter.MakeGenericMethod(entityType, property.PropertyType);
                var propertyGetterExpression = thisGetPropertyGetter.Invoke(null, new object[] { property.Name });

                orderedResults = (IOrderedQueryable<TResult>)orderByMethod.Invoke(null, new[] { orderedResults ?? results, propertyGetterExpression })!;
            }
        }
        orderedResults ??= GetDefaultOrderBy(results);

        var result = await CreatePagedResults(parameters, orderedResults);
        if (result.TotalCount > 0)
            result = await AddTotals(parameters, orderedResults, result);
        return result;
    }

    private static class GenericsSupport
    {
        public static Expression<Func<TEntity, TProperty>> GetPropertyGetter<TEntity, TProperty>(string propName)
        {
            var parameter = Expression.Parameter(typeof(TEntity));
            var property = Expression.Property(parameter, propName);
            return Expression.Lambda<Func<TEntity, TProperty>>(property, parameter);
        }
    }

    protected virtual Task<TPagedResults> AddTotals(TParameters parameters, IOrderedQueryable<TResult> orderedResults, TPagedResults result) 
        => Task.FromResult(result);

    protected abstract IQueryable<TResult> GetQuery(TParameters parameters);

    private static async Task<TPagedResults> CreatePagedResults(TParameters parameters, IOrderedQueryable<TResult> orderedQuery)
    {
        var count = await orderedQuery.CountAsync();
        var results = count == 0 ? Array.Empty<TResult>() 
            : await orderedQuery
            .Skip(parameters.PageSize * (parameters.Page - 1)).Take(parameters.PageSize)
            .ToArrayAsync();

        var result = new TPagedResults { Parameters = parameters, TotalCount = count, Results=results };
        return result;
    }

    protected abstract IOrderedQueryable<TResult> GetDefaultOrderBy(IQueryable<TResult> query);

}