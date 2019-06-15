using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestProiectLicenta.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class UserChooseCarToSellPage : ContentPage
    {
        private List<Car> ListItems;

        public UserChooseCarToSellPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await PopulateList();
        }

        private async Task PopulateList()
        {
            var userId = await SecureStorage.GetAsync("UserId");
            ListItems = await App.CarManager.GetUserCars(Convert.ToInt32(userId));

            list.ItemsSource = ListItems;
        }
    }
}
