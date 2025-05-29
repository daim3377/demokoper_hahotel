using System.Data.Common;
using Utilities.Models;

namespace DEMO_KOZLOV.DataBase.Abstractions;

public interface IDBContext : IDisposable
{
    public DbCommand CreateCommand(string sql);

    public Exception? Command(string sql);

    public FullResponse<object?> Scalar(string sql);

    public FullResponse<T?> ReaderWrapper<T>(string sql, Func<DbDataReader, T> action);
}