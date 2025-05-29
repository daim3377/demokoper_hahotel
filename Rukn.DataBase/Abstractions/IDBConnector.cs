using System.Data.Common;

namespace DEMO_KOZLOV.DataBase.Abstractions;

public interface IDBConnector
{
    public DbConnection Connect();
}
