using Npgsql;
using NpgsqlTypes;
using RGR.Models.Entities;
using System.Collections.Generic;

namespace RGR.Models;

public class HotelModel : Model<Hotel>
{
    public HotelModel(NpgsqlConnection connection) : base(connection) { }

    public override void CreateItem(Hotel item)
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.Parameters.Add(new NpgsqlParameter("@param_name", NpgsqlDbType.Varchar, 100) { Value = (object?)item.Hotel_Name });
        command.Parameters.Add(new NpgsqlParameter("@param_address", NpgsqlDbType.Varchar, 100) { Value = (object?)item.Address });
        command.Parameters.Add(new NpgsqlParameter("@param_rating", NpgsqlDbType.Integer) { Value = (object?)item.Star_Rating });
        command.Parameters.Add(new NpgsqlParameter("@param_journey", NpgsqlDbType.Bigint) { Value = (object?)item.Journey_ID });

        command.CommandText = $"explain analyze insert into \"Hotels\"(\"Hotel_Name\", \"Address\", \"Star_Rating\", \"Journey_ID\") values(@param_name, @param_address, @param_rating, @param_journey);";

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

        command.CommandText = $"explain analyze delete from \"Hotels\" where \"Hotel_ID\" = {id}";

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

        string query = $"insert into \"Hotels\"(\"Hotel_Name\", \"Address\", \"Star_Rating\", \"Journey_ID\") \r\nvalues \r\n(\r\n\tchr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int),  \r\n\tchr(trunc(65 + random() * 25)::int) || chr(trunc(65 + random() * 25)::int),  \r\n\ttrunc(1 + random() * 5)::int, \r\n\t(select f_id from trunc(1 + random() * (select max(\"Journey_ID\") from \"Journeys\")) as f_id inner join \"Journeys\" on f_id = \"Journeys\".\"Journey_ID\")::bigint\r\n);";

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

    public override IEnumerable<Hotel> ReadItems()
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.CommandText = $"select * from \"Hotels\"";

        IEnumerable<Hotel> items = new List<Hotel>();

        try
        {
            Connection.Open();

            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Hotel item = new Hotel();

                item.Hotel_ID = (long)reader["Hotel_ID"];
                item.Hotel_Name = (string)reader["Hotel_Name"];
                item.Address = (string)reader["Address"];
                item.Star_Rating = (int)reader["Star_Rating"];
                item.Journey_ID = (long)reader["Journey_ID"];

                ((List<Hotel>)items).Add(item);
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

    public override void UpdateItem(long id, Hotel newItem)
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.Parameters.Add(new NpgsqlParameter("@param_name", NpgsqlDbType.Varchar, 100) { Value = (object?)newItem.Hotel_Name });
        command.Parameters.Add(new NpgsqlParameter("@param_address", NpgsqlDbType.Varchar, 100) { Value = (object?)newItem.Address });
        command.Parameters.Add(new NpgsqlParameter("@param_rating", NpgsqlDbType.Integer) { Value = (object?)newItem.Star_Rating });
        command.Parameters.Add(new NpgsqlParameter("@param_journey", NpgsqlDbType.Bigint) { Value = (object?)newItem.Journey_ID });

        command.CommandText = $"explain analyze update \"Hotels\" set \"Hotel_ID\" = {id}, \"Hotel_Name\" = @param_name, \"Address\" = @param_address, \"Star_Rating\" = @param_rating, \"Journey_ID\" = @param_journey where \"Hotel_ID\" = {id};";

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

    public IEnumerable<object> ReadHotelFlightCount(long filterF, long filterS)
    {
        using NpgsqlCommand command = new NpgsqlCommand();

        command.Connection = Connection;

        command.CommandText = $"select \"Hotels\".\"Hotel_ID\", \"Hotels\".\"Hotel_Name\", count(\"Flights\".\"Flight_ID\") as \"Flight_Count\"\r\nfrom \"Hotels\"\r\ninner join \"Hotel-Flights\" using (\"Hotel_ID\")\r\ninner join \"Flights\" using (\"Flight_ID\")\r\ngroup by \"Hotels\".\"Hotel_ID\", \"Hotels\".\"Hotel_Name\", \"Hotels\".\"Star_Rating\"\r\nhaving count(\"Flights\".\"Flight_ID\") between {filterF} and {filterS}\r\nOrder by \"Hotels\".\"Hotel_ID\";";

        IEnumerable<object> items = new List<object>();

        try
        {
            Connection.Open();

            using NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ((List<object>)items).Add(new
                {
                    Hotel_ID = (long)reader["Hotel_ID"],
                    Hotel_Name = (string)reader["Hotel_Name"],
                    Flight_Count = (long)reader["Flight_Count"]
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
