using Npgsql;
using NpgsqlTypes;
using RGR.Models.Entities;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace RGR.Models;

public class ClientModel : Model<Client>
{
    public ClientModel(NpgsqlConnection connection) : base(connection) {}

    public override void CreateItem(Client item)
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.Parameters.Add(new NpgsqlParameter("@param_first_name", NpgsqlDbType.Varchar, 100) { Value = (object?)item.First_Name });
        command.Parameters.Add(new NpgsqlParameter("@param_last_name", NpgsqlDbType.Varchar, 100) { Value = (object?)item.Last_Name });
        command.Parameters.Add(new NpgsqlParameter("@param_mail", NpgsqlDbType.Varchar, 256) { Value = (object?)item.Email });

        command.CommandText = $"explain analyze insert into \"Clients\"(\"First_Name\", \"Last_Name\", \"Email\") values(@param_first_name, @param_last_name, @param_mail);";

        try
        {
            Connection.Open();

            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) 
            {
                LastQueryExecutionTime = (string)reader.GetValue(0);
            }
            
        }
        finally
        {
            Connection.Close();
        }
    }

    public override void DeleteItem(long id)
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.CommandText = $"delete from \"Clients\" where \"Client_ID\" = {id}";

        try
        {
            Connection.Open();

            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                LastQueryExecutionTime = (string)reader.GetValue(0);
            }
        }
        finally
        {
            Connection.Close();
        }
    }

    public override IEnumerable<Client> ReadItems()
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.CommandText = $"select * from \"Clients\"";

        IEnumerable<Client> items = new List<Client>();

        try
        {
            Connection.Open();

            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) 
            {
                Client item = new Client();

                item.Client_ID = (long)reader["Client_ID"];
                item.First_Name = (string)reader["First_Name"];
                item.Last_Name = (string)reader["Last_Name"];
                item.Email = (string)reader["Email"];

                ((List<Client>)items).Add(item);
            }
        }
        finally
        {
            Connection.Close();
        }

        using var explain = command.Clone();
        explain.CommandText = "explain analyze " + command.CommandText;

        try
        {
            Connection.Open();

            using NpgsqlDataReader explainReader = explain.ExecuteReader();

            while (explainReader.Read())
            {
                LastQueryExecutionTime = (string)explainReader.GetValue(0);
            }
        }
        finally
        {
            Connection.Close();
        }

        return items;
    }

    public override void UpdateItem(long id, Client newItem)
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.Parameters.Add(new NpgsqlParameter("@param_first_name", NpgsqlDbType.Varchar, 100) { Value = (object?)newItem.First_Name });
        command.Parameters.Add(new NpgsqlParameter("@param_last_name", NpgsqlDbType.Varchar, 100) { Value = (object?)newItem.Last_Name });
        command.Parameters.Add(new NpgsqlParameter("@param_mail", NpgsqlDbType.Varchar, 256) { Value = (object?)newItem.Email });

        command.CommandText = $"explain analyze update \"Clients\" set \"Client_ID\" = {id}, \"First_Name\" = @param_first_name, \"Last_Name\" = @param_last_name, \"Email\" = @param_mail where \"Client_ID\" = {id};";
        
        try
        {
            Connection.Open();

            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                LastQueryExecutionTime = (string)reader.GetValue(0);
            }
        }
        finally
        {
            Connection.Close();
        }
    }

    public override void GenerateItems(long count)
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        string query = $"insert into \"Clients\"(\"First_Name\", \"Last_Name\", \"Email\") values\r\n(chr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int),\r\nchr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int),\r\nchr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int));";

        command.CommandText = "";

        for (int i = 0; i < count; i++)
            command.CommandText += query;

        try
        {
            Connection.Open();

            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                LastQueryExecutionTime = (string)reader.GetValue(0);
            }

        }
        finally
        {
            Connection.Close();
        }
    }
}
