using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace Countdown
{
    public partial class HistoryPage : ContentPage
    {
        // Collection to hold game history items
        public ObservableCollection<GameResult> GameHistory { get; set; }

        public HistoryPage()
        {
            InitializeComponent(); // Initialize UI components
            LoadHistory(); // Load the game history
            historyListView.ItemsSource = GameHistory; // Set the data source for the ListView
        }

        // Load game history from the manager
        private void LoadHistory()
        {
            var historyManager = new GameHistoryManager(); // Create a new GameHistoryManager
            var history = historyManager.LoadGameHistory(); // Load the game history list

            // Convert the list to an ObservableCollection for data binding
            GameHistory = new ObservableCollection<GameResult>(history);
        }
    }
}
