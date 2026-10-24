using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Курсовой_проект.ViewModels;

namespace Курсовой_проект.Services
{
    public class SqliteDialogueRepository : IDialogueRepository
    {
        public List<DialogueStep> GetStoryContent()
        {
            var story = new List<DialogueStep>();
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "бд", "sweetcandies.db");
            string connectionString = $"Data Source={dbPath}";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT c.Name, d.Text, d.BackgroundPath, d.MusicPath, d.SpritePath
                    FROM Dialogues d
                    LEFT JOIN Characters c ON d.CharacterId = c.Id
                    ORDER BY d.Id";

                using (var reader = command.ExecuteReader())
                {
                    int idCounter = 1;
                    while (reader.Read())
                    {
                        story.Add(new DialogueStep
                        {
                            Id = idCounter++,
                            CharacterName = reader.IsDBNull(0) ? "" : reader.GetString(0),
                            Text = reader.GetString(1),
                            BackgroundPath = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            MusicPath = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            SpritePath = reader.IsDBNull(4) ? "" : reader.GetString(4)
                        });
                    }
                }
            }
            return story;
        }
    }
}