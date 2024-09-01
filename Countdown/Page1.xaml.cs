using Microsoft.Maui.Controls;

namespace Countdown
{
    // Defines a partial class for Page1 that inherits from ContentPage
    public partial class Page1 : ContentPage
    {
        // Constructor for Page1
        public Page1()
        {
            InitializeComponent(); // Initializes the page components
        }

        // Handles the Play button click event
        private async void clickPlay(object sender, EventArgs e)
        {
            // Navigates asynchronously to the Players page
            await Navigation.PushAsync(new Players());
        }

        // Handles the History button click event
        private async void OnHistoryClicked(object sender, EventArgs e)
        {
            // Navigates asynchronously to the HistoryPage
            await Navigation.PushAsync(new HistoryPage());
        }

        // Handles the Settings button click event
        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            // Navigates asynchronously to the SettingsPage
            await Navigation.PushAsync(new SettingsPage());
        }

        // Handles the Exit button click event
        private void clickExit(object sender, EventArgs e)
        {
            // Exits the application
            System.Environment.Exit(0);
        }
    }
}
