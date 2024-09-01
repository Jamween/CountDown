using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json; // For JSON handling

namespace Countdown
{
    public class GameHistoryManager
    {
        private const string FileName = "gameHistory.json"; // File name for storing game history
        private string filePath; // Path to the file

        private readonly Formatting jsonFormatting = Formatting.Indented; // JSON format style

        public GameHistoryManager()
        {
            filePath = Path.Combine(FileSystem.AppDataDirectory, FileName); // Set the full path for the file
        }

        // Load the game history from the file
        public List<GameResult> LoadGameHistory()
        {
            if (!File.Exists(filePath)) // If the file does not exist
            {
                return new List<GameResult>(); // Return an empty list
            }

            var json = File.ReadAllText(filePath); // Read the file content
            return JsonConvert.DeserializeObject<List<GameResult>>(json) ?? new List<GameResult>(); // Deserialize the JSON
        }

        // Save the game history to the file
        public void SaveGameHistory(List<GameResult> history)
        {
            var json = JsonConvert.SerializeObject(history, jsonFormatting); // Serialize the history
            File.WriteAllText(filePath, json); // Write the JSON to the file
        }

        // Add a new game result to the history
        public void AddGameResult(GameResult result)
        {
            var history = LoadGameHistory(); // Load existing history
            history.Add(result); // Add the new result
            SaveGameHistory(history); // Save the updated history
        }
    }
}
