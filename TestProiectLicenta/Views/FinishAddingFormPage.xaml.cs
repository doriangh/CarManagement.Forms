using System;
using TestProiectLicenta.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class FinishAddingFormPage : ContentPage
    {
        private Car newUserCar;
        private CarDetail newCarDetail;
        private CarImages newCarImage;

        public FinishAddingFormPage(Car car = null, CarDetail carDetail = null, CarImages carImage = null)
        {
            InitializeComponent();

            newUserCar = car;
            newCarDetail = carDetail;
            newCarImage = carImage;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            carimage.Source = newCarImage.CarImage;
            carname.Text = newUserCar.FullName;
            year.Text = newUserCar.ModelYear;
            type.Text = newUserCar.Body;
            fuel.Text = newUserCar.Fuel;
            power.Text = newUserCar.Power;
            cc.Text = newUserCar.Cc;
            odometer.Text = newUserCar.Odometer;
            license.Text = newUserCar.License;

            itp.Text = (newCarDetail.Itp.AddYears(2) - DateTime.Today).Days.ToString() + " days left until the next ITP";
            roadtax.Text = (newCarDetail.RoadTax.AddYears(1) - DateTime.Today).Days.ToString() + " days left until roadtax renewal";
            oilchange.Text = newCarDetail.OilChange + " KM left until oil change";
            tyres.Text = "Winter tyres are " + (newCarDetail.WinterTires ? "ON" : "OFF");

        }

        private async void Submit_Button(object sender, EventArgs e)
        { 
            var userId = await SecureStorage.GetAsync("UserId");

            newUserCar.UserId = Convert.ToInt32(userId);

            var added = await App.CarManager.AddCar(newUserCar);

            if (added)
            {
                var cars = await App.CarManager.GetUserCars(Convert.ToInt32(userId));

                var carId = cars[cars.Count - 1].Id;

                newCarDetail.CarId = carId;
                newCarImage.carId = carId;

                App.CarImagesManager.AddCarImages(newCarImage);
                await App.CarDetailManager.AddCarDetail(newCarDetail);

                await Navigation.PopToRootAsync();
            }
            else
            {
                await DisplayAlert("Error", "Could not add car", "OK");
            }

        }

        async void Go_Back_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}