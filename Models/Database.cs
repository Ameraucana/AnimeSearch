using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AnimeSearch.Models
{
    public static class Database
    {
        public enum Table {
            mediaList,
            search
        }
        private static string connectionString = "DataSource=./Assets/Files/data.db";

        public static void ReadTable(Table selection)
        {
            string table = selection == Table.mediaList ?
                                            "mediaList" :
                                            "search";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                        SELECT * FROM $table;
                    ";
                command.Parameters.AddWithValue("$table", table);
                
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        List<MediaItem> mediaList = new List<MediaItem>();
                        while (reader.Read())
                        {
                            if (selection == Table.mediaList)
                            {
                                object[] values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                MediaItem newMedia = new MediaItem
                                {
                                    Name = (string)values[0],
                                    Progress = (int)values[1],
                                    Episodes = (int)values[2],
                                    ReleaseType = (string)values[3],
                                    StartDate = (string)values[4],
                                    Status = (string)values[5],
                                    Rating = (int)values[6]
                                };
                                mediaList.Add(newMedia);
                            } else if (selection == Table.search)
                            {

                            }
                        }
                    }
                }
            }
        }

        public static async Task WriteMediaList(List<MediaItem> items)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var deleteCommand = connection.CreateCommand();
                deleteCommand.CommandText =
                    @"
                        DELETE FROM mediaList;
                    ";
                await deleteCommand.ExecuteNonQueryAsync();

                foreach (MediaItem mediaItem in items)
                {
                    var fillCommand = connection.CreateCommand();
                    fillCommand.CommandText =
                        @"
                            INSERT INTO mediaList 
                                        (name, progress, episodes, releaseType,
                                         startDate, status, rating)
                                        VALUES ($name, $progress, $episodes, $releaseType,
                                                $startDate, $status, $rating);
                        ";

                    var parameters = new SqliteParameter[]
                    {
                        new SqliteParameter("$name", mediaItem.Name),
                        new SqliteParameter("$progress", mediaItem.Progress),
                        new SqliteParameter("$episodes", mediaItem.Episodes ?? 0),
                        new SqliteParameter("$releaseType", mediaItem.ReleaseType),
                        new SqliteParameter("$startDate", mediaItem.StartDate),
                        new SqliteParameter("$status", mediaItem.Status),
                        new SqliteParameter("$rating", mediaItem.Rating),
                    };
                    fillCommand.Parameters.AddRange(parameters);
                    await fillCommand.ExecuteNonQueryAsync();
                }

            }
        }
    }
}
