using System;

namespace BLE.Client.Pages
{
    public partial class LoginPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

		async void OnButtonClicked(object sender, EventArgs args)
		{
			await DisplayAlert("Enter Password", "Now enter your password with the sensor tag", "Cancel");
		}
    }
}
