using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using TestProiectLicenta.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class CarDetailsSetupFormPage : ContentPage
    {
        CarVinRequest _carRequest;

        public CarDetailsSetupFormPage(CarVinRequest car = null)
        {
            InitializeComponent();

            if (car == null) 

                _carRequest = new CarVinRequest
                {
                    Car = new Car(),
                    Errors = new List<string>()
                };

            else _carRequest = car;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_carRequest.Car.CarImage != null) image.Source = _carRequest.Car.CarImage;

            make.Text = _carRequest.Car.Make;
            model.Text = _carRequest.Car.Model;
            year.Text = _carRequest.Car.ModelYear;
            type.Items.Add(_carRequest.Car.Body);
            fuel.Text = _carRequest.Car.Fuel;
            odometer.Text = _carRequest.Car.Odometer;
            License.Text = _carRequest.Car.License;
        }

        async void AddImageButton(object sender, System.EventArgs e)
        {
            var action = await DisplayActionSheet("How would you like to add the image?", "Cancel", null, "Take Photo", "Select Photo");

            if (action == "Take Photo")
            {
                _carRequest = await App.ExternalAPIManager.HandleTakingPicture(_carRequest);

                if (_carRequest.Success)
                {
                    image.Source = _carRequest.Car.CarImage;
                }
                else
                {
                    await DisplayAlert("Error", _carRequest.Errors[0], "OK");
                }
            }
            else if (action == "Select Photo")
            {
                _carRequest = await App.ExternalAPIManager.HandleSelectionPicture(_carRequest);

                if (_carRequest.Success)
                {
                    image.Source = _carRequest.Car.CarImage;
                }
                else
                {
                    await DisplayAlert("Error", _carRequest.Errors[0], "OK");
                }
            }
        }

        async void CancelButton(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void ContinueButton(object sender, System.EventArgs e)
        {

            _carRequest.Car.Make = make.Text;
            _carRequest.Car.Model = model.Text;
            _carRequest.Car.ModelYear = year.Text;
            _carRequest.Car.Body = type.SelectedItem.ToString();
            _carRequest.Car.Fuel = fuel.Text;
            _carRequest.Car.Odometer = odometer.Text;

            await App.CarManager.AddCar(_carRequest.Car);

            var userId = await SecureStorage.GetAsync("UserID");

            var cars = await App.CarManager.GetUserCars(Convert.ToInt32(userId));

            var carId = cars[cars.Count].Id;

            if (itp.Date >= DateTime.Today && roadtax.Date >= DateTime.Today && oilchange.Date >= DateTime.Today)
            {
                CarDetail carDetail = new CarDetail
                {
                    CarId = carId,
                    ITP = itp.Date,
                    RoadTax = roadtax.Date,
                    OilChange = oilchange.Date,
                    WinterTires = wintertires.IsEnabled
                };
                await App.CarDetailManager.AddCarDetail(carDetail);
            }


            await Navigation.PushAsync(new UserPageForm());
        }
    }
}
