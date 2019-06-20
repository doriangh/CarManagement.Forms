using System;
using System.Threading.Tasks;
using TestProiectLicenta.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class FinishAddingFormPage : ContentPage
    {
        private readonly Car _newUserCar;
        private readonly CarDetail _newCarDetail;
        private readonly CarImages _newCarImage;

        public FinishAddingFormPage(Car car = null, CarDetail carDetail = null, CarImages carImage = null)
        {
            InitializeComponent();

            _newUserCar = car;
            _newCarDetail = carDetail;
            _newCarImage = carImage;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            carimage.Source = _newCarImage.CarImage;
            carname.Text = _newUserCar.FullName;
            year.Text = _newUserCar.ModelYear;
            type.Text = _newUserCar.Body;
            fuel.Text = _newUserCar.Fuel;
            power.Text = _newUserCar.Power;
            cc.Text = _newUserCar.Cc;
            odometer.Text = _newUserCar.Odometer;
            license.Text = _newUserCar.License;

            itp.Text = (_newCarDetail.Itp.AddYears(2) - DateTime.Today).Days.ToString();
            roadtax.Text = (_newCarDetail.RoadTax.AddYears(1) - DateTime.Today).Days.ToString();
            oilchange.Text = _newCarDetail.OilChange + " KM left until oil change";
            tyres.Text = (_newCarDetail.WinterTires ? "on" : "off");

        }

        private async void Submit_Button(object sender, EventArgs e)
        { 
            var userId = await SecureStorage.GetAsync("UserId");

            _newUserCar.UserId = Convert.ToInt32(userId);

            var added = await App.CarManager.AddCar(_newUserCar);

            if (added)
            {
                var cars = await App.CarManager.GetUserCars(Convert.ToInt32(userId), true);

                var carId = cars[cars.Count - 1].Id;

                _newCarDetail.CarId = carId;
                _newCarImage.carId = carId;

                await Task.WhenAll(App.CarImagesManager.AddCarImages(_newCarImage), App.CarDetailManager.AddCarDetail(_newCarDetail));
                //await Task.WhenAll(App.CarDetailManager.AddCarDetail(_newCarDetail), App.CarImagesManager.AddCarImages(_newCarImage));

                await Navigation.PopToRootAsync();
            }
            else
            {
                await DisplayAlert("Error", "Could not add car", "OK");
            }

            }

        private async void Go_Back_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}