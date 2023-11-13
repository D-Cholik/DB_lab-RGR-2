using Npgsql;

namespace RGR.Models;

public abstract class Model<T> where T : class, new()
{
    public NpgsqlConnection Connection { get; private set; }

    public Model(NpgsqlConnection connection)
    {
        Connection = connection;
    }
    public abstract void CreateItem(T item);

    public abstract IEnumerable<T> ReadItems();

    public abstract void UpdateItem(long id, T newItem);

    public abstract void DeleteItem(long id);

    public abstract void GenerateItems(long count);

    public string LastQueryExecutionTime { get; protected set; }
}
