using System.Data.Common;

namespace DEMO_KOZLOV.DataBase.Models;

public class TransactionDBContext : DBContext
{
    private DbTransaction Transaction;
    public TransactionDBContext(DbConnection connection, Action? dispose) : base(connection, dispose)
    {
        Transaction = connection.BeginTransaction();
    }

    public override void Dispose()
    {
        Transaction.Commit();
        Transaction.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}
