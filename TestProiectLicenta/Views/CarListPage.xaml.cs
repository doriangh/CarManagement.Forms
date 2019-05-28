using System;
using System.Collections.Generic;
using TestProiectLicenta.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class CarListPage : ContentPage
    {
        public CarListPage()
        {
            InitializeComponent();
        }

        private async void Handle_Clicked(object sender, System.EventArgs e)
        {
            var userId = await SecureStorage.GetAsync("UserId");

            var cars = await App.CarManager.GetUserCars(Convert.ToInt32(userId));

            var listData = new List<ListCarAttributes>();

            foreach (var car in cars)
            {
                var carDetail = await App.CarDetailManager.GetCarsDetail(Convert.ToInt32(car.Id));

                if (carDetail == null) continue;
                var listCar = new ListCarAttributes
                {
                    FullName = car.FullName,
                    ModelYear = car.ModelYear,
                    RemainingItp = (carDetail.Itp - DateTime.Today).TotalDays.ToString(),
                    RemainingRoadTax = (carDetail.RoadTax - DateTime.Today).TotalDays.ToString(),
                    RemainingOilChange = (carDetail.OilChange - DateTime.Today).TotalDays.ToString(),
                    RoadTaxValue = carDetail.RoadTaxValue.ToString(),
                    InsuranceValue = carDetail.InsuranceValue.ToString()
                };

                listData.Add(listCar);

            }

            //list.ItemsSource = cars;
            //list.ItemsSource = cars;
            list.ItemsSource = listData;
            list.RowHeight = 200;
        }

        private void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}