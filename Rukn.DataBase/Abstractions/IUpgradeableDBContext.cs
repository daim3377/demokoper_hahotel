namespace DEMO_KOZLOV.DataBase.Abstractions;

public interface IUpgradeableDBContext : IDBContext
{
    IDBContext Write();
    IDBContext Transaction();
}