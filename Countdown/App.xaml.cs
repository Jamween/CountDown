using Microsoft.Maui.Controls;

namespace Countdown
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

           
            MainPage = new NavigationPage(new Page1());
        }
    }
}
