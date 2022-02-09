using System.Data.Common;

namespace IntelligenceReporting.Databases;

public interface IDatabase
{
    Task<T[]> Query<T>(string sql, Func<DbDataReader, T> readRow);
    Task Query(string sql, Action<DbDataReader> readRow);
    Task Query(string sql, Func<DbDataReader, Task> readRow);
}

public interface IVaultDatabase : IDatabase { }

/// <summary>A second vault database for when nested queries</summary>
public interface IVaultDatabase2 : IDatabase { }
