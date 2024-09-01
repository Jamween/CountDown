namespace Countdown
{
    // Defines a partial class for Players that inherits from ContentPage
    public partial class Players : ContentPage
    {
        // Static variables to hold player names
        public static string name1;
        public static string name2;

        // Constructor for Players
        public Players()
        {
            InitializeComponent(); // Initializes the page components defined in Players.xaml
        }

        // Handles the Submit button click event
        public async void clickSubmit(object sender, EventArgs e)
        {
            // Retrieve player names from the text fields
            name1 = Player1Name.Text;
            name2 = Player2Name.Text;

            // Check if Player 1's name is empty or whitespace
            if (string.IsNullOrWhiteSpace(name1))
            {
                // Display an alert if Player 1's name is missing
                await DisplayAlert("Empty name", "Please enter a name for Player 1", "Ok");
                return; // Exit the method early
            }

            // Check if Player 2's name is empty or whitespace
            if (string.IsNullOrWhiteSpace(name2))
            {
                // Display an alert if Player 2's name is missing
                await DisplayAlert("Empty name", "Please enter a name for Player 2", "Ok");
                return; // Exit the method early
            }

            // Disable the button to prevent multiple submissions
            nameBtn.IsEnabled = false;

            // Navigate asynchronously to MainPage
            await Navigation.PushAsync(new MainPage());

            // Optionally re-enable the button in case of navigation failure
            nameBtn.IsEnabled = true;
        }
    }
}
