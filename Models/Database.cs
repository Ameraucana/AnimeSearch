using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using System.Diagnostics;

namespace AnimeSearch.Models
{
    public static class Database
    {
        private static string connectionString = "DataSource=./Assets/Files/data.db";

        public static void ReadTable()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                        SELECT * FROM mediaList;
                    ";

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            object[] values = new object[reader.FieldCount];
                            reader.GetValues(values);
                            foreach (object value in values)
                            {
                                Debug.WriteLine(value);
                            }
                        }
                    }
                }
            }
        }

        public static void WriteRow()
        {

        }
    }
}
