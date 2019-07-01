using System;
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
        private List<string> _errors = new List<string>();
        private CarVinRequest _carRequest;
        private CarImages _newCarImage;

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
            type.SelectedItem = _carRequest.Car.Body ?? null;
            fuel.SelectedItem = _carRequest.Car.Fuel ?? null;
            odometer.Text = _carRequest.Car.Odometer;
            License.Text = _carRequest.Car.License;
            cc.Text = _carRequest.Car.Cc;

            itp.MinimumDate = DateTime.Today.AddYears(-2);
            roadtax.MinimumDate = DateTime.Today.AddYears(-1);
            insurancedate.MinimumDate = DateTime.Today.AddYears(-1);
        }

        private async void AddImageButton(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("How would you like to add the image?", "Cancel", null, "Take Photo",
                "Select Photo");

            switch (action)
            {
                case "Take Photo":
                    {
                        _carRequest = await App.ExternalAPIManager.HandleTakingPicture(_carRequest);   

                        if (_carRequest.Success)
                        {
                            UserDialogs.Instance.Toast("Done!");
                            image.Source = _carRequest.Car.CarImage;
                            _newCarImage = new CarImages
                            {
                                CarImage = _carRequest.Car.CarImage
                            };
                        }
                        else
                            await DisplayAlert("Error", _carRequest.Errors[0], "OK");

                        break;
                    }

                case "Select Photo":
                    {
                        _carRequest = await App.ExternalAPIManager.HandleSelectionPicture(_carRequest);

                        if (_carRequest.Success)
                        {
                            UserDialogs.Instance.Toast("Done!");
                            image.Source = _carRequest.Car.CarImage;
                            _newCarImage = new CarImages
                            {
                                CarImage = _carRequest.Car.CarImage
                            };
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
            if (make.Text == null) { make.Text = "Please enter a car make"; make.LabelColor = Color.Red; }
            else _carRequest.Car.Make = make.Text;

            if (model.Text == null) { model.Text = "Please enter a car model"; model.LabelColor = Color.Red; }
            else _carRequest.Car.Model = model.Text;

            if (year.Text == null) { year.Text = "Please enter a car model year"; model.LabelColor = Color.Red; }
            else _carRequest.Car.ModelYear = year.Text;

            if (type.SelectedItem == null) { type.SelectedItem = "Please enter a type"; type.TextColor = Color.Red; }
            else _carRequest.Car.Body = type.SelectedItem.ToString();

            if (fuel.SelectedItem == null) { fuel.SelectedItem = "Please enter the car's fuel"; fuel.TextColor = Color.Red; }
            else _carRequest.Car.Fuel = fuel.SelectedItem.ToString();

            if (power.Text == null) { power.Text = "Please enter the power for the car"; model.LabelColor = Color.Red; }
            else _carRequest.Car.Power = power.Text;

            if (odometer.Text == null) { odometer.Text = "Please enter the odometer"; model.LabelColor = Color.Red; }
            else _carRequest.Car.Odometer = odometer.Text;

            if (cc.Text == null) { cc.Text = "Please enter the car's cc"; cc.LabelColor = Color.Red; }
            else _carRequest.Car.Cc = cc.Text;

            if (License.Text == null) { License.Text = "Please enter the car's licence"; License.LabelColor = Color.Red; }
            else _carRequest.Car.License = License.Text;

            if (_newCarImage == null)
                _newCarImage = new CarImages();

            if (_carRequest.Car.CarImage == null && _newCarImage.CarImage == null)
            {

                var url = "http://www.carimagery.com/api.asmx/GetImageUrl?searchTerm=" + _carRequest.Car.Make + "+" +
                          _carRequest.Car.Model + "+" + _carRequest.Car.ModelYear ??
                          null + "+" + _carRequest.Car.Body ?? null;

                var reader = new XmlTextReader(url);

                while (reader.Read())
                    if (reader.Value.Contains("http://"))
                        //carImage.Source = ImageSource.FromUri(new Uri(reader.Value.Trim()));
                        _newCarImage.CarImage = reader.Value.Trim();
                //Console.WriteLine(reader.Value.Trim());
                _carRequest.Car.CarImage = _newCarImage.CarImage;
            }
            else if (_carRequest.Car.CarImage == null && _newCarImage.CarImage != null)
            {
                _carRequest.Car.CarImage = _newCarImage.CarImage;
            }
            else
            {
                _newCarImage.CarImage = _carRequest.Car.CarImage;
            }

            //_carRequest.Car.CarImage = _newCarImage.CarImage;

            var carDetail = new CarDetail
            {
                Itp = itp.Date,
                RoadTax = roadtax.Date,
                RoadTaxPeriod = CalculateRoadTaxPeriod(),
                RoadTaxValue = CalculateRoadTax(),
                Insurance = insurancedate.Date,
                InsurancePeriod = CalculateInsurancePeriod(),
                OilChange = Convert.ToInt32(odometer.Text) % 15000,
                WinterTires = wintertires.On,
                TaxValue = CalculateTax(_carRequest)
            };


            await Navigation.PushAsync(new FinishAddingFormPage(_carRequest.Car, carDetail, _newCarImage));
        }

        private int CalculateRoadTaxPeriod()
        {
            switch (roadtaxDuration.SelectedIndex)
            {
                case 0:
                    return 7;
                case 1:
                    return 30;
                case 2:
                    return 90;
                default:
                    return 365;

            }
        }

        private int CalculateInsurancePeriod()
        {
            switch (insuranceDuration.SelectedIndex)
            {
                case 0:
                    return 6;
                case 1:
                    return 12;
                default:
                    return 0;
            }
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

        private int CalculateRoadTax()
        {
            switch (roadtaxDuration.SelectedIndex)
            {
                case 0:
                    return 3;
                case 1:
                    return 7;
                case 2:
                    return 13;
                case 3:
                    return 28;
                default:
                    return 0;
            }
        }

        private void Handle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fuel.SelectedIndex >= 0)
            {
                var selectedItem = fuel.Items[fuel.SelectedIndex];

                if (selectedItem == "Electric")
                {
                    cc.IsEnabled = false;
                    cc.Placeholder = "Not available on electric cars";
                    _carRequest.Car.Cc = "0";
                }
                else
                {
                    cc.IsEnabled = true;
                    cc.Placeholder = "eg. 1600";
                }
            }
        }
    }
}
