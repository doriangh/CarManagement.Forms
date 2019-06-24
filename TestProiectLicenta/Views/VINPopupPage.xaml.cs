using System;
using Acr.UserDialogs;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Views
{
    public partial class VinPopupPage : PopupPage
    {
        Car _car;

        public VinPopupPage(Car car = null)
        { 
            InitializeComponent();

            _car = car;
        }

        private async void Cancel_Button(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            if (_car != null)
            {
                var car = await App.ExternalAPIManager.GetCarByVin(vinEntry.Text);
                if (_car.Model == null) _car.Model = car.Car.Model;
                if (_car.Make == null) _car.Make = car.Car.Make;
                if (_car.ModelYear == null) _car.ModelYear = car.Car.ModelYear;

                

                _car.Vin = vinEntry.Text;
                var success = await App.CarManager.UpdateCar(_car.Id, _car);
                if (success)
                    UserDialogs.Instance.Toast("Succesfully updated VIN");
                else
                    UserDialogs.Instance.Toast("Error updating");
                await Navigation.PopPopupAsync();
            }
            else
            {
                var newCar = await App.ExternalAPIManager.GetCarByVin(vinEntry.Text);
                if (newCar.Success)
                {
                    await Navigation.PushAsync(new FillExtraFormPage(newCar));
                    await Navigation.PopPopupAsync();
                }
            }
        }
    }
}