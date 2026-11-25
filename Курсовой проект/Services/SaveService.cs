using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Diagnostics;

namespace Курсовой_проект.Services
{
    public class SaveService : ISaveService
    {
        private string GetConnectionString()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "novel.db");
            return $"Data Source={dbPath}";
        }

        private void EnsureTableCreated(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Saves (
                    SlotId INTEGER PRIMARY KEY,
                    StepIndex INTEGER,
                    SaveDate TEXT
                );";
            command.ExecuteNonQuery();
        }

        public bool SaveGame(int slotId, int stepIndex)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    EnsureTableCreated(connection);

                    var command = connection.CreateCommand();
                    command.CommandText = @"
                        INSERT OR REPLACE INTO Saves (SlotId, StepIndex, SaveDate)
                        VALUES (@slotId, @stepIndex, @saveDate);";

                    command.Parameters.AddWithValue("@slotId", slotId);
                    command.Parameters.AddWithValue("@stepIndex", stepIndex);
                    command.Parameters.AddWithValue("@saveDate", DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка сохранения: {ex.Message}");
                return false;
            }
        }

        public int LoadGame(int slotId)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    EnsureTableCreated(connection);

                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT StepIndex FROM Saves WHERE SlotId = @slotId;";
                    command.Parameters.AddWithValue("@slotId", slotId);

                    var result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int stepIndex))
                    {
                        return stepIndex;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка загрузки слота {slotId}: {ex.Message}");
            }

            return -1;
        }

        public string GetSaveDate(int slotId)
        {
            try
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    EnsureTableCreated(connection);

                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT SaveDate FROM Saves WHERE SlotId = @slotId;";
                    command.Parameters.AddWithValue("@slotId", slotId);

                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        return $"Слот {slotId}: {result}";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка чтения даты слота {slotId}: {ex.Message}");
            }

            return $"Слот {slotId}: Пусто";
        }
    }
}