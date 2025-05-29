namespace DEMO_KOZLOV.DataBase.Abstractions;

public interface IDBTable
{
    public void Create();
    public void Fill();
    public void Drop();
}