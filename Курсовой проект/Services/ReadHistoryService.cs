using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace Курсовой_проект.Services
{
    public static class ReadHistoryService
    {
        private static readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "read_history.json");
        private static HashSet<int> _readSteps = new HashSet<int>();

        public static void Load()
        {
            if (!File.Exists(_filePath)) return;

            try
            {
                string json = File.ReadAllText(_filePath);
                var list = JsonSerializer.Deserialize<List<int>>(json);
                if (list != null)
                {
                    _readSteps = new HashSet<int>(list);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка загрузки истории: {ex.Message}");
            }
        }

        public static void Save()
        {
            try
            {
                var list = new List<int>(_readSteps);
                string json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка сохранения истории: {ex.Message}");
            }
        }

        public static bool IsRead(int stepId)
        {
            return _readSteps.Contains(stepId);
        }

        public static void MarkAsRead(int stepId, bool saveImmediately = true)
        {
            if (_readSteps.Add(stepId) && saveImmediately)
            {
                Save();
            }
        }

        public static void Clear()
        {
            _readSteps.Clear();
            Save();
        }
    }
}