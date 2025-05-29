using System.Data.Common;
using DEMO_KOZLOV.DataBase.Abstractions;
using Utilities.Models;

namespace DEMO_KOZLOV.DataBase.Models;

public class DBContext : IDBContext
{
    protected DbConnection _connection;
    protected Action? _dispose;

    public DBContext(DbConnection connection, Action? dispose)
    {
        _connection = connection;
        _dispose = dispose;
    }

    public DbCommand CreateCommand(string sql)
    {
        DbCommand command = _connection.CreateCommand();
        command.CommandText = sql;
        return command;
    }

    public Exception? Command(string sql)
    {
        try
        {
            using DbCommand command = CreateCommand(sql);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            return ex;
        }
        return null;
    }

    public FullResponse<object?> Scalar(string sql)
    {
        try
        {
            using DbCommand command = CreateCommand(sql);
            return new(Response.Success, null, command.ExecuteScalar(), null);
        }
        catch (Exception ex)
        {
            return new(Response.Fail, null, null, ex);
        }
    }

    public FullResponse<T?> ReaderWrapper<T>(string sql, Func<DbDataReader, T> action)
    {
        try
        {
            using DbCommand command = CreateCommand(sql);
            using DbDataReader reader = command.ExecuteReader();
            return new(Response.Success, null, action(reader), null);
        }
        catch (Exception ex)
        {
            return new(Response.Fail, null, default, ex);
        }
    }

    public virtual void Dispose()
    {
        _dispose?.Invoke();
        GC.SuppressFinalize(this);
    }
}
