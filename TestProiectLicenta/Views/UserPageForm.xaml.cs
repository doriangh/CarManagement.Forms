using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class UserPageForm : TabbedPage
    {
        public UserPageForm()
        {
            InitializeComponent();
        }

        private async void AddCarsWhenNoCarsButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new ChooseMethodFormPage()));
        }

        private async void BackButtonPressed(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SignOutButton(object sender, EventArgs e)
        {
            SecureStorage.RemoveAll();

            await Navigation.PushAsync(new UserLoginPage());
        }

        private void Handle_Toggled(object sender, ToggledEventArgs e)
        {
            Application.Current.Properties["FaceID"] = Fid.On;
        }
    }
}