using System;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class UserPageForm : TabbedPage
    {
        public UserPageForm()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void AddCarsWhenNoCarsButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new ChooseMethodFormPage()));
        }

        private async void BackButtonPressed(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }      
    }
}