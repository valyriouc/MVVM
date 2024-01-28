using Microsoft.EntityFrameworkCore;

using MySqlConnector;

using System.Data;
using System.Data.Common;

namespace BusinessLogic;

public interface IDatabaseAccessable
{
    public abstract static IDatabaseAccessable FromDatabase(DbDataReader reader);
}

public class User : IDatabaseAccessable
{
    public int UserId { get; private set; }

    public string Username { get; private set; }

    public string Password { get; private set; } 

    public User(int userId, string username, string password)
    {
        UserId = userId;
        Username = username;
        Password = password;
    }

    public static IDatabaseAccessable FromDatabase(DbDataReader reader)
    {
        int userId = reader.GetInt32(0);
        string name = reader.GetString(1);
        string password = reader.GetString(2);

        return new User(userId, name, password);
    }
}

public class DbAccess : IDisposable
{
    private static string ConnectionString => "Data Source=localhost;port=3306;Initial Catalog=mvvm;UserId=username;Password=password";

    private static DbAccess? instance;

    public static DbAccess Instance
    {
        get
        {
            if (instance is null)
            {
                instance = new DbAccess();
            }

            return instance;
        }
    }

    private DbConnection connection;

    private DbAccess()
    {
        connection = new MySqlConnection(ConnectionString); 
        if (connection.State == ConnectionState.Closed)
        {
            connection.Open();
        }
    }

    public async IAsyncEnumerable<TEntity> GetAsync<TEntity>(string query)
        where TEntity : IDatabaseAccessable
    {
        DbCommand command = connection.CreateCommand();
        command.CommandText = query;

        using DbDataReader reader = await command.ExecuteReaderAsync();

        while(reader.NextResult())
        {
            yield return (TEntity)TEntity.FromDatabase(reader);
        }
    }

    public void Create(string query)
    {
        DbCommand command = connection.CreateCommand();
        command.CommandText = query;

        command.ExecuteNonQuery();
    }

    public void Dispose()
    {
        connection.Dispose();
    }
}
