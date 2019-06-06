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

        public VinPopupPage(Car car)
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
            _car.Vin = vinEntry.Text;
            var success = await App.CarManager.UpdateCar(_car.Id, _car);
            if (success)
                UserDialogs.Instance.Toast("Succesfully updated VIN");
            else
                UserDialogs.Instance.Toast("Error updating");
            await Navigation.PopPopupAsync();

        }
    }
}