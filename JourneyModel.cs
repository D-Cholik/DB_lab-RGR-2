using Npgsql;
using NpgsqlTypes;
using RGR.Models.Entities;

namespace RGR.Models;

public class JourneyModel : Model<Journey>
{
    public JourneyModel(NpgsqlConnection connection) : base(connection) { }

    public override void CreateItem(Journey item)
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.Parameters.Add(new NpgsqlParameter("@param_name", NpgsqlDbType.Varchar, 100) { Value = (object?)item.Journey_Name });
        command.Parameters.Add(new NpgsqlParameter("@param_date", NpgsqlDbType.Timestamp) { Value = (object?)item.Departure_Date });
        command.Parameters.Add(new NpgsqlParameter("@param_cost", NpgsqlDbType.Money) { Value = (object?)item.Cost });
        command.Parameters.Add(new NpgsqlParameter("@param_client", NpgsqlDbType.Bigint) { Value = (object?)item.Client_ID });

        command.CommandText = $"explain analyze insert into \"Journeys\"(\"Journey_Name\", \"Departure_Date\", \"Cost\", \"Client_ID\") values(@param_name, @param_date, @param_cost, @param_client);";

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

        command.CommandText = $"explain analyze delete from \"Journeys\" where \"Journey_ID\" = {id}";

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

        string query = $"insert into \"Journeys\"(\"Journey_Name\", \"Departure_Date\", \"Cost\", \"Client_ID\")\r\nvalues \r\n(\r\n\tchr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int),\r\n\t(select timestamp '2020-01-10 20:00:00' + random() * (timestamp '2023-01-10 10:00:00' - timestamp '2020-01-10 20:00:00'))::timestamp, \r\n\ttrunc(1 + random() * 2000)::int,\r\n\t(select f_id from trunc(1 + random() * (select max(\"Client_ID\") from \"Clients\")) as f_id inner join \"Clients\" on f_id = \"Clients\".\"Client_ID\")::bigint\r\n);";

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

    public override IEnumerable<Journey> ReadItems()
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.CommandText = $"select * from \"Journeys\"";

        IEnumerable<Journey> items = new List<Journey>();

        try
        {
            Connection.Open();

            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Journey item = new Journey();

                item.Journey_ID = (long)reader["Journey_ID"];
                item.Journey_Name = (string)reader["Journey_Name"];
                item.Departure_Date = (DateTime)reader["Departure_Date"];
                item.Cost = (decimal)reader["Cost"];
                item.Client_ID = (long)reader["Client_ID"];

                ((List<Journey>)items).Add(item);
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

    public override void UpdateItem(long id, Journey newItem)
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.Parameters.Add(new NpgsqlParameter("@param_name", NpgsqlDbType.Varchar, 100) { Value = (object?)newItem.Journey_Name });
        command.Parameters.Add(new NpgsqlParameter("@param_date", NpgsqlDbType.Timestamp) { Value = (object?)newItem.Departure_Date });
        command.Parameters.Add(new NpgsqlParameter("@param_cost", NpgsqlDbType.Money) { Value = (object?)newItem.Cost });
        command.Parameters.Add(new NpgsqlParameter("@param_client", NpgsqlDbType.Bigint) { Value = (object?)newItem.Client_ID });

        command.CommandText = $"explain analyze update \"Journeys\" set \"Journey_ID\" = {id}, \"Journey_Name\" = @param_name, \"Departure_Date\" = @param_date, \"Cost\" = @param_cost, \"Client_ID\" = @param_client where \"Journey_ID\" = {id};";

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

    public IEnumerable<object> ReadJourneyClients(long filter)
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.CommandText = $"select \"Journey_ID\", \"Journey_Name\", \"Client_ID\", \"First_Name\", \"Last_Name\" \r\nfrom \"Journeys\"\r\ninner join \"Clients\" using (\"Client_ID\")\r\nwhere \"Journey_ID\" > {filter}\r\nOrder by \"Journey_ID\";";

        IEnumerable<object> items = new List<object>();

        try
        {
            Connection.Open();

            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ((List<object>)items).Add(new
                {
                    Journey_ID = (long)reader["Journey_ID"],
                    Journey_Name = (string)reader["Journey_Name"],
                    Client_ID = (long)reader["Client_ID"],
                    First_Name = (string)reader["First_Name"],
                    Last_Name = (string)reader["Last_Name"],
                });
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
}
