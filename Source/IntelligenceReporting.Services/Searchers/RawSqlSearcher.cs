using System.Data.Common;
using System.Diagnostics;
using IntelligenceReporting.Databases;
using IntelligenceReporting.Queries;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IntelligenceReporting.Searchers;

/// <summary>A base class for raw sql searching</summary>
/// <remarks>
/// This should only be used other options aren't possible
/// particularly if common table expressions are required
/// </remarks>
public abstract class RawSqlSearcher<TParameters, TResult, TPagedResults> : IDisposable
    where TParameters : QueryParameters, new()
    where TPagedResults : PagedResults<TParameters, TResult>, new()
{
    private readonly ILogger _logger;
    private readonly IntelligenceReportingDbContext _dbContext;
    private SqlConnection? _dbConnection;

    protected RawSqlSearcher(IntelligenceReportingDbContext dbContext, ILogger logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    private async Task<SqlConnection> GetDbConnection()
    {
        if (_dbConnection != null) return _dbConnection;

        _dbConnection = new SqlConnection(_dbContext.Database.GetConnectionString());
        await _dbConnection.OpenAsync();
        return _dbConnection;
    }

    public void Dispose()
    {
        _dbConnection?.Dispose();

        GC.SuppressFinalize(this);
    }

    public async Task<TPagedResults> Query(TParameters parameters)
    {
        var results = new TPagedResults { Parameters = parameters };

        var cteSql = BuildSqlCommonTableExpressions(parameters);
        var detailSql = BuildDetailSql(parameters);

        var orderByText = parameters.OrderBy + "," + GetDefaultOrderBys();
        var orderBys = new List<string>();
        foreach (var order in orderByText.Split(',').Select(s => s.Trim()).Where(s => s != ""))
        {
            var lastSpace = order.LastIndexOf(' ');
            var propertyName = lastSpace == -1 ? order : order[..lastSpace];
            var isDescending = lastSpace > -1 && new[] { 'D', 'd' }.Contains(order[lastSpace + 1]);
            orderBys.Add(propertyName + (isDescending ? " desc" : ""));
        }
        var orderBy = $"ORDER BY {string.Join(", ", orderBys.Distinct())}";

        var detailsPageSql = $@"
{cteSql}
{detailSql}
{orderBy}
";
        results.Results = await Query(detailsPageSql, ReadDetail);
        results.TotalCount = results.Results.Length;

        await OnQueryComplete(results);
        return results;
    }

    protected abstract string GetDefaultOrderBys();

    protected async Task<T> QueryOne<T>(string sql, Func<DbDataReader, T> readRow)
    {
        var results = await Query(sql, readRow);
        return results[0];
    }

    protected async Task<T[]> Query<T>(string sql, Func<DbDataReader, T> readRow)
    {
        var results = new List<T>();
        await Query(sql, row =>
        {
            results.Add(readRow(row));
        });
        return results.ToArray();
    }

    protected async Task Query(string sql, Action<DbDataReader> readRow)
    {
        await Query(sql, reader =>
        {
            readRow(reader);
            return Task.CompletedTask;
        });
    }

    protected async Task Query(string sql, Func<DbDataReader, Task> readRow)
    {
        _logger.LogInformation("Executing \r\n{sql}", sql);
        var sw = Stopwatch.StartNew();
        var rows = 0;
        var dbConnection = await GetDbConnection();
        await using (var command = new SqlCommand(sql, dbConnection))
        await using (var reader = await command.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                rows++;
                await readRow(reader);
            }
        }

        _logger.LogInformation(" - Read {rows} rows in {seconds:#,##0.000}.", rows, sw.Elapsed.TotalSeconds);
    }


    protected virtual string BuildSqlCommonTableExpressions(TParameters parameters) => "";
    protected abstract string BuildDetailSql(TParameters parameters);
    protected abstract TResult ReadDetail(DbDataReader reader);
    protected virtual Task OnQueryComplete(TPagedResults results) => Task.CompletedTask;

}