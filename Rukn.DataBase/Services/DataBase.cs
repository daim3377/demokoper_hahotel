using System.Data.Common;
using DEMO_KOZLOV.DataBase.Abstractions;
using DEMO_KOZLOV.DataBase.Models;

namespace DEMO_KOZLOV.DataBase.Services;

public class DataBase : IDataBase
{
    protected DbConnection _connection;
    protected ReaderWriterLockSlim _locker;

    public DataBase(IDBConnector connector, ReaderWriterLockSlim? locker)
    {
        _locker = locker ?? new ReaderWriterLockSlim();
        _connection = connector.Connect();
        _connection.Open();
    }

    public DataBase(DbConnection connection, ReaderWriterLockSlim? locker)
    {
        _locker = locker ?? new ReaderWriterLockSlim();
        _connection = connection;
        _connection.Open();
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
        _locker.Dispose();
        GC.SuppressFinalize(this);
    }

    public IDBContext Read()
    {
        _locker.EnterReadLock();
        return new DBContext(_connection, _locker.ExitReadLock);
    }

    public IDBContext Write()
    {
        _locker.EnterWriteLock();
        return new DBContext(_connection, _locker.ExitWriteLock);
    }

    public IUpgradeableDBContext UpgradeableRead()
    {
        _locker.EnterUpgradeableReadLock();
        return new UpgradeableDBContext(this, _connection, _locker.ExitUpgradeableReadLock);
    }

    public IDBContext Transaction()
    {
        _locker.EnterWriteLock();
        return new TransactionDBContext(_connection, _locker.ExitWriteLock);
    }
}