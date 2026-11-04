using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Курсовой_проект.Models.Курсовой_проект.Models;
using System.IO;

namespace Курсовой_проект.Services
{
    public static class SettingsService
    {
        private static readonly string _filePath = "settings.json";

        public static GameSettings Current { get; private set; } = new GameSettings();

        public static void Load()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                Current = JsonSerializer.Deserialize<GameSettings>(json) ?? new GameSettings();
            }
        }

        public static void Save()
        {
            string json = JsonSerializer.Serialize(Current, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
