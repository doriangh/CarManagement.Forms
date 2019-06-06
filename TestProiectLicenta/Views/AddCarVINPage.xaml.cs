using System;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class AddCarVinPage : ContentPage
    {
        public AddCarVinPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            error.IsVisible = false;
        }

        private async void FindCarVinButton(object sender, EventArgs e)
        {
            var vin = VINField.Text;

            if (vin != null)
            {
                var data = await App.ExternalAPIManager.GetCarByVin(vin);

                await Navigation.PushAsync(new CarDetailsSetupFormPage(data));
            }
            else if (vin.Length > 14 || vin.Length < 14)
            {
                error.IsVisible = true;
            }
            else
            {
                error.Text = "Please enter VIN";
                error.IsVisible = true;
            }
        }
    }
}