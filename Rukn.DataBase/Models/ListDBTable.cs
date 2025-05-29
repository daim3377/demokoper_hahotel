using DEMO_KOZLOV.DataBase.Abstractions;
using DEMO_KOZLOV.DataBase.Extensions;
using Utilities.Models;

namespace DEMO_KOZLOV.DataBase.Models;

public class ListDBTable : IDBTable
{
    protected readonly IDataBase _db;
    protected readonly string _tableName;

    protected ListDBTable(IDataBase db, string tableName)
    {
        _db = db;
        _tableName = tableName;
    }

    public void Create() => _db.Write($@"CREATE TABLE {_tableName}(
	id INT PRIMARY KEY AUTO_INCREMENT,
	text VARCHAR(255) UNIQUE NOT NULL
)");

    public void Drop() => _db.Write(_tableName);

    public virtual void Fill() { }

    public void Add(string text, int? id = null)
    {
        if (id.HasValue)
            _db.Write($"REPLACE INTO {_tableName}(id, text) VALUES({id.Value}, '{text}')");
        else
            _db.Write($"INSERT INTO {_tableName}(text) VALUES('{text}')");
    }

    public void Add(List<string> data)
    {
        using IDBContext context = _db.Write();
        foreach (string item in data)
            context.Command($"INSERT INTO {_tableName}(text) VALUES('{item}')");
    }

    public void Update(int id, string text)
        => _db.Write($"UPDATE {_tableName} SET text = '{text}' WHERE id = {id}");

    public void RemoveById(int id)
        => _db.Write($"DELETE FROM {_tableName} WHERE id = {id}");

    public void RemoveByText(string text)
        => _db.Write($"DELETE FROM {_tableName} WHERE text = '{text}'");

    public FullResponse<string?> GetTextById(int id)
        => _db.Scalar($"SELECT text FROM {_tableName} WHERE id = {id}").As(x => (string?)x);

    public FullResponse<int?> GetIdByText(string text)
        => _db.Scalar($"SELECT id FROM {_tableName} WHERE text = '{text}'").As(x => (int?)x);

    public FullResponse<List<int>?> GetIds(int limit = 100, int offset = 0)
        => _db.ReaderWrapper($"SELECT id FROM {_tableName} LIMIT {limit} OFFSET {offset}", DBHelper.GetAllNumbers);

    public FullResponse<List<string>?> GetList(int limit = 100, int offset = 0)
        => _db.ReaderWrapper($"SELECT text FROM {_tableName} LIMIT {limit} OFFSET {offset}", DBHelper.GetAllStrings);

    public FullResponse<Dictionary<int,string>?> GetDictionary(int limit = 100, int offset = 0)
        => _db.ReaderWrapper($"SELECT id, text FROM {_tableName} LIMIT {limit} OFFSET {offset}", DBHelper.GetIdStringPairs);

    public FullResponse<int?> Count()
        => _db.Scalar("SELECT COUNT(*) FROM " + _tableName).As(x=> (int?) (x is null ? null : Convert.ToInt32(x)));
}
