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

                if (_car.Model == null) _car.Model = car.Car.Model ?? null;
                if (_car.Make == null) _car.Make = car.Car.Make ?? null;
                if (_car.ModelYear == null) _car.ModelYear = car.Car.ModelYear ?? null;
                if (_car.Body == null) _car.Body = car.Car.Body ?? null;
                if (_car.Color == null) _car.Color = car.Car.Color ?? null;
                if (_car.Fuel == null) _car.Fuel = car.Car.Fuel ?? null;
                if (_car.Cc == null) _car.Cc = car.Car.Cc ?? null;
                if (_car.Power == null) _car.Power = car.Car.Power ?? null;
                if (_car.Odometer == null) _car.Odometer = car.Car.Odometer ?? null;
                if (_car.Manufacturer == null) _car.Manufacturer = car.Car.Manufacturer ?? null;
                if (_car.Plant == null) _car.Plant = car.Car.Plant ?? null;
                if (_car.SequentialNumber == null) _car.SequentialNumber = car.Car.SequentialNumber ?? null;
                if (_car.Drive == null) _car.Drive = car.Car.Drive ?? null;
                if (_car.NumberofSeats == null) _car.NumberofSeats = car.Car.NumberofSeats ?? null;
                if (_car.NumberofDoors == null) _car.NumberofDoors = car.Car.NumberofDoors ?? null;
                if (_car.Steering == null) _car.Steering = car.Car.Steering ?? null;
                if (_car.EngineDisplacement == null) _car.EngineDisplacement = car.Car.EngineDisplacement ?? null;
                if (_car.EngineCylinders == null) _car.EngineCylinders = car.Car.EngineCylinders ?? null;
                if (_car.NumberofGears == null) _car.NumberofGears = car.Car.NumberofGears ?? null;
                if (_car.Engine == null) _car.Engine = car.Car.Engine ?? null;
                if (_car.Made == null) _car.Made = car.Car.Made ?? null;
                if (_car.Emissions == null) _car.Emissions = car.Car.Emissions ?? null;
                

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