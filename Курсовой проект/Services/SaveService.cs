using System;
using Microsoft.Data.Sqlite;

namespace Курсовой_проект.Services
{
    public class SaveService
    {
        private string GetConnectionString()
        {
            return "Data Source=novel.db";
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

        public void SaveGame(int slotId, int stepIndex)
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
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка сохранения: {ex.Message}");
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
            catch (Exception)
            {
                
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
                        return $"Слот {slotId}: {result.ToString()}";
                    }
                }
            }
            catch (Exception)
            {
                
            }

            return $"Слот {slotId}: Пусто";
        }
    }
}