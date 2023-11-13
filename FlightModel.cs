using Npgsql;
using NpgsqlTypes;
using RGR.Models.Entities;
using System.Collections.Generic;

namespace RGR.Models;

public class FlightModel : Model<Flight>
{
    public FlightModel(NpgsqlConnection connection) : base(connection) { }

    public override void CreateItem(Flight item)
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.Parameters.Add(new NpgsqlParameter("@param_number", NpgsqlDbType.Integer) { Value = (object?)item.Flight_Number });
        command.Parameters.Add(new NpgsqlParameter("@param_departure", NpgsqlDbType.Varchar, 100) { Value = (object?)item.Departure });
        command.Parameters.Add(new NpgsqlParameter("@param_destination", NpgsqlDbType.Varchar, 256) { Value = (object?)item.Destination });

        command.CommandText = $"explain analyze insert into \"Flights\"(\"Flight_Number\", \"Departure\", \"Destination\") values(@param_number, @param_departure, @param_destination);";

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

        command.CommandText = $"delete from \"Flights\" where \"Flight_ID\" = {id}";

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

        string query = $"insert into \"Flights\"(\"Flight_Number\", \"Departure\", \"Destination\") values \r\n(trunc(random() * 10000)::int, \r\nchr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int), \r\nchr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int));";

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

    public override IEnumerable<Flight> ReadItems()
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.CommandText = $"select * from \"Flights\"";

        IEnumerable<Flight> items = new List<Flight>();

        try
        {
            Connection.Open();

            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Flight item = new Flight();

                item.Flight_ID = (long)reader["Flight_ID"];
                item.Flight_Number = (int)reader["Flight_Number"];
                item.Departure = (string)reader["Departure"];
                item.Destination = (string)reader["Destination"];

                ((List<Flight>)items).Add(item);
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

    public override void UpdateItem(long id, Flight newItem)
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.Parameters.Add(new NpgsqlParameter("@param_number", NpgsqlDbType.Integer) { Value = (object?)newItem.Flight_Number });
        command.Parameters.Add(new NpgsqlParameter("@param_departure", NpgsqlDbType.Varchar, 100) { Value = (object?)newItem.Departure });
        command.Parameters.Add(new NpgsqlParameter("@param_destination", NpgsqlDbType.Varchar, 256) { Value = (object?)newItem.Destination });

        command.CommandText = $"explain analyze update \"Flights\" set \"Flight_ID\" = {id}, \"Flight_Number\" = @param_number, \"Departure\" = @param_departure, \"Destination\" = @param_destination where \"Flight_ID\" = {id};";

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

    public IEnumerable<object> ReadFlightClientCount(long filter)
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.CommandText = $"select \"Flights\".\"Flight_ID\", \"Flights\".\"Flight_Number\", count(\"Client_ID\") as u_c\r\nfrom \"Flights\"\r\ninner join \"Hotel-Flights\" using (\"Flight_ID\")\r\ninner join \"Hotels\" using (\"Hotel_ID\")\r\ninner join \"Journeys\" using (\"Journey_ID\")\r\ninner join \"Clients\" using (\"Client_ID\")\r\nwhere \"Flights\".\"Flight_Number\" > {filter}\r\ngroup by \"Flights\".\"Flight_ID\", \"Flights\".\"Flight_Number\", \"Flights\".\"Departure\", \"Flights\".\"Destination\"\r\nOrder by \"Flights\".\"Flight_ID\";";
        IEnumerable<object> items = new List<object>();



        try
        {
            Connection.Open();

            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ((List<object>)items).Add(new
                {
                    Flight_ID = (long)reader["Flight_ID"],
                    Flight_Number = (int)reader["Flight_Number"],
                    Client_Count = (long)reader["u_c"]
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
