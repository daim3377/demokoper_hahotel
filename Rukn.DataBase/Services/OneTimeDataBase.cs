using System.Data.Common;
using DEMO_KOZLOV.DataBase.Abstractions;
using DEMO_KOZLOV.DataBase.Models;

namespace DEMO_KOZLOV.DataBase.Services;

public class OneTimeDatabase : IDataBase
{
    protected Func<DbConnection> _factory;

    public OneTimeDatabase(IDBConnector connector) => _factory = connector.Connect;

    public OneTimeDatabase(Func<DbConnection> factory) => _factory = factory;

    public void Dispose() => GC.SuppressFinalize(this);

    private DBContext Connect()
    {
        DbConnection connection = _factory.Invoke();
        return new DBContext(connection, async () => { await connection.CloseAsync(); connection.Dispose(); });
    }

    public IDBContext Read() => Connect();

    public IDBContext Write() => Connect();

    public IUpgradeableDBContext UpgradeableRead()
    {
        DbConnection connection = _factory.Invoke();
        return new UpgradeableDBContext(this, connection, async () => { await connection.CloseAsync(); connection.Dispose(); });
    }

    public IDBContext Transaction()
    {
        DbConnection connection = _factory.Invoke();
        return new TransactionDBContext(connection, async () => { await connection.CloseAsync(); connection.Dispose(); });
    }
}
