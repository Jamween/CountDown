using Microsoft.Maui.Controls;

namespace Countdown
{
    public partial class SettingsPage : ContentPage
    {
        // Constructor for the SettingsPage class
        public SettingsPage()
        {
            InitializeComponent(); // Initializes the components defined in the XAML file
        }

        // Event handler for the Save Settings button click event
        private void SaveSettings(object sender, EventArgs e)
        {
            // Retrieves the current values from the sliders
            var fontSize = fontSizeSlider.Value; // Gets the font size value from the slider
            var timerLength = timerLengthSlider.Value; // Gets the timer length value from the slider
            var numberOfRounds = roundsSlider.Value; // Gets the number of rounds value from the slider

            // Display a confirmation alert to the user
            DisplayAlert("Settings Saved", "Your settings have been saved.", "OK");
        }
    }
}
