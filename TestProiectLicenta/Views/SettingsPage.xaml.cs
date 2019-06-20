using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            Fid.IsToggled = Convert.ToBoolean(Application.Current.Properties["FaceID"].ToString());
        }

        private async void SignOutButton(object sender, EventArgs e)
        {
            SecureStorage.RemoveAll();

            await Navigation.PushAsync(new UserLoginPage());
        }

        private void Handle_Toggled(object sender, ToggledEventArgs e)
        {
            Application.Current.Properties["FaceID"] = Fid.IsToggled;
        }
    }
}
