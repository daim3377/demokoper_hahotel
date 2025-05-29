using System.Data.Common;
using DEMO_KOZLOV.DataBase.Abstractions;
using Utilities.Models;

namespace DEMO_KOZLOV.DataBase.Extensions;

public static class DBHelper
{
    public static Exception? Write(this IDataBase db, string sql)
    {
        using IDBContext ctx = db.Write();
        return ctx.Command(sql);
    }

    public static FullResponse<T?> ReaderWrapper<T>(this IDataBase db, string sql, Func<DbDataReader, T> action)
    {
        using IDBContext ctx = db.Read();
        return ctx.ReaderWrapper(sql, action);
    }

    public static FullResponse<T?> ReaderWrapper<T>(this IDBContext context, string sql, Func<DbDataReader, IDBContext, T> action)
        => context.ReaderWrapper(sql, reader => action(reader, context));

    public static FullResponse<T?> ReaderWrapper<T>(this IDataBase db, string sql, Func<DbDataReader, IDBContext, T> action)
    {
        using IDBContext ctx = db.Read();
        return ctx.ReaderWrapper(sql, action);
    }

    public static FullResponse<object?> Scalar(this IDataBase db, string sql)
    {
        using IDBContext ctx = db.Read();
        return ctx.Scalar(sql);
    }

    public static (DateTime start, DateTime end) DateToWeek(DateTime date)
    {
        while (date.DayOfWeek != DayOfWeek.Monday)
            date = date.AddDays(-1);
        return (date, date.AddDays(6));
    }

    public static string CreateConditionByDate(DateTime start, DateTime? end)
        => end.HasValue ? $"date BETWEEN '{start:yyyy-MM-dd}' AND '{end:yyyy-MM-dd}'" : $"date='{start:yyyy-MM-dd}'";

    public static List<string>? GetAllStrings(this DbDataReader reader)
    {
        if (!reader.HasRows)
            return null;

        var result = new List<string>();

        while (reader.Read())
            result.Add(reader.GetString(0));

        return result;
    }

    public static List<int>? GetAllNumbers(this DbDataReader reader)
    {
        if (!reader.HasRows)
            return null;

        var result = new List<int>();

        while (reader.Read())
            result.Add(reader.GetInt32(0));

        return result;
    }

    public static Dictionary<int, string>? GetIdStringPairs(this DbDataReader reader)
    {
        if (!reader.HasRows)
            return null;

        var result = new Dictionary<int, string>();

        while (reader.Read())
            result.Add(reader.GetInt32(0), reader.GetString(1));

        return result;
    }
}
