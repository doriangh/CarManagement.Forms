﻿using System;
using System.Collections.Generic;
using System.Xml;
using Acr.UserDialogs;
using TestProiectLicenta.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class FillExtraFormPage : ContentPage
    {
        private List<string> errors = new List<string>();
        private CarVinRequest _carRequest;

        public FillExtraFormPage(CarVinRequest car = null)
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
            type.SelectedItem = _carRequest.Car.Body;
            fuel.SelectedItem = _carRequest.Car.Fuel;
            odometer.Text = _carRequest.Car.Odometer;
            License.Text = _carRequest.Car.License;
            cc.Text = _carRequest.Car.Cc;
        }

        //private async void Continue_button(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new FinishAddingFormPage());
        //}

        private async void AddImageButton(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("How would you like to add the image?", "Cancel", null, "Take Photo",
                "Select Photo");

            switch (action)
            {
                case "Take Photo":
                    {
                        using (UserDialogs.Instance.Toast(" Uploading image..."))
                        {
                            _carRequest = await App.ExternalAPIManager.HandleTakingPicture(_carRequest);
                        }

                        if (_carRequest.Success)
                        {
                            UserDialogs.Instance.Toast("Done!");
                            image.Source = _carRequest.Car.CarImage;
                        }
                        else
                            await DisplayAlert("Error", _carRequest.Errors[0], "OK");

                        break;
                    }

                case "Select Photo":
                    {
                        using (UserDialogs.Instance.Toast(" Uploading image..."))
                        {
                            _carRequest = await App.ExternalAPIManager.HandleSelectionPicture(_carRequest);
                        }
                        if (_carRequest.Success)
                        {
                            UserDialogs.Instance.Toast("Done!");
                            image.Source = _carRequest.Car.CarImage;
                        }
                        else
                            await DisplayAlert("Error", _carRequest.Errors[0], "OK");

                        break;
                    }
            }
        }

        private async void CancelButton(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void ContinueButton(object sender, EventArgs e)
        {
            _carRequest.Car.Make = make.Text;
            _carRequest.Car.Model = model.Text;
            _carRequest.Car.ModelYear = year.Text;
            _carRequest.Car.Body = type.SelectedItem.ToString();
            _carRequest.Car.Fuel = fuel.SelectedItem.ToString();
            _carRequest.Car.Odometer = odometer.Text;
            _carRequest.Car.Cc = cc.Text;

            if (_carRequest.Car.CarImage == null)
            {
                var url = "http://www.carimagery.com/api.asmx/GetImageUrl?searchTerm=" + _carRequest.Car.Make + "+" +
                          _carRequest.Car.Model + "+" + _carRequest.Car.ModelYear ??
                          null + "+" + _carRequest.Car.Body ?? null;

                var reader = new XmlTextReader(url);

                while (reader.Read())
                    if (reader.Value.Contains("http://"))
                        //carImage.Source = ImageSource.FromUri(new Uri(reader.Value.Trim()));
                        _carRequest.Car.CarImage = reader.Value.Trim();
                //Console.WriteLine(reader.Value.Trim());
            }

            var userId = await SecureStorage.GetAsync("UserId");

            _carRequest.Car.UserId = Convert.ToInt32(userId);

            await App.CarManager.AddCar(_carRequest.Car);

            var cars = await App.CarManager.GetUserCars(Convert.ToInt32(userId));

            var carId = cars[cars.Count - 1].Id;

            var carDetail = new CarDetail
            {
                CarId = carId,
                Itp = itp.Date,
                RoadTax = roadtax.Date,
                OilChange = oilchange.Date,
                WinterTires = wintertires.IsEnabled,
                TaxValue = CalculateTax(_carRequest)
            };


            await App.CarDetailManager.AddCarDetail(carDetail);


            await Navigation.PushAsync(new UserPageForm());
        }

        private static int CalculateTax(CarVinRequest car)
        {
            if (Convert.ToInt32(car.Car.Cc) == 0) return Convert.ToInt32(0);
            if (Convert.ToInt32(car.Car.Cc) <= 1600)
                return Convert.ToInt32(car.Car.Cc) / 200 * 8;
            if (Convert.ToInt32(car.Car.Cc) <= 2000)
                return Convert.ToInt32(car.Car.Cc) / 200 * 18;
            if (Convert.ToInt32(car.Car.Cc) <= 2600)
                return Convert.ToInt32(car.Car.Cc) / 200 * 72;
            if (Convert.ToInt32(car.Car.Cc) <= 3000)
                return Convert.ToInt32(car.Car.Cc) / 200 * 144;
            if (Convert.ToInt32(car.Car.Cc) > 3001)
                return Convert.ToInt32(car.Car.Cc) / 200 * 290;
            return -1;
        }
        //void Handle_Completed(object sender, System.EventArgs e)
        //{
        //    //if (Convert.ToInt32(year.Text) < 1970 || Convert.ToInt32(year.Text) > DateTime.Today.Year)
        //    //{
        //    //} 
        //}

        private void Handle_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = fuel.SelectedItem.ToString();

            if (selectedItem == "Electric")
            {
            }
        }
    }
}