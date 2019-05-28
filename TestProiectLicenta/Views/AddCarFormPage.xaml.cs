using System;
using TestProiectLicenta.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class AddCarFormPage : ContentPage
    {
        public AddCarFormPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void SaveDataButton(object sender, System.EventArgs e)
        { 
            if (name.Text != null && model.Text != null && year.Text != null && fuel.Text != null)
            {
                var id = await SecureStorage.GetAsync("UserId");

                var car = new Car
                {
                    UserId = Convert.ToInt32(id),
                    Make = name.Text,
                    Model = model.Text,
                    ModelYear = year.Text,
                    Fuel = fuel.Text,
                    Body = type.SelectedItem.ToString()
                };

                await App.CarManager.AddCar(car);
            }

            await Navigation.PushAsync(new UserPageForm());
        }
    }
}
