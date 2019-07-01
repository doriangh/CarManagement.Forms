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
            Task.WhenAll(PopulateList());
        }

        private async Task PopulateList()
        {
            var userId = await SecureStorage.GetAsync("UserId");
            ListItems = await App.CarManager.GetUserCars(Convert.ToInt32(userId), true);

            list.ItemsSource = ListItems;
        }

        async void Sell_Car_Tap(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            var listItem = e.SelectedItem as Car;

            await Navigation.PushModalAsync(new SellDetailsModalPage(listItem));

            ((ListView)sender).SelectedItem = null;
        }
    }
}
