using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class BuyCarPage : ContentPage
    {
        private List<CarsSold> _allCarsSold;
        private List<CarsSoldListItem> _listItems;

        public BuyCarPage()
        {
            InitializeComponent();
            Task.WhenAll(PopulateList());
        }

        private async Task PopulateList(bool force = false)
        {
            _allCarsSold = await App.CarsSoldManager.GetAll(force);
            _listItems = new List<CarsSoldListItem>();

            foreach (var carSold in _allCarsSold)
            {
                var car = await App.CarManager.GetCar(carSold.CarId, force);

                var carItem = new CarsSoldListItem
                {
                    CarId = car.Id,
                    FullName = car.FullName,
                    ShortDescription = carSold.Details,
                    ModelYear = Convert.ToInt32(car.ModelYear),
                    Odometer = Convert.ToInt32(car.Odometer),
                    Price = Convert.ToInt32(car.CarPrice),
                    Cc = Convert.ToInt32(car.Cc),
                    Power = Convert.ToInt32(car.Power),
                    Fuel = car.Fuel,
                    VIN = car.Vin,
                    Color = car.Color,
                    LongDescription = carSold.LongDescription,
                    CarImage = car.CarImage
                };

                _listItems.Add(carItem);
            }

            list.ItemsSource = _listItems;
        }

        private async void Handle_Refreshing(object sender, System.EventArgs e)
        {
            await Task.WhenAll(PopulateList(true));
            list.IsRefreshing = false;
        }

        private async void User_Add_Car_Button(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new UserChooseCarToSellPage());
        }

        private async void Filter_List_Button(object sender, System.EventArgs e)
        {
            var action = await DisplayActionSheet("How would you like to add the image?", "Cancel", null, "A -> Z",
                "Z -> A", "By Power", "By Odometer");

            switch (action)
            {
                case "A -> Z":
                    list.ItemsSource = _listItems.OrderBy(c => c.FullName);
                    break;
                case "Z -> A":
                    list.ItemsSource = _listItems.OrderByDescending(c => c.FullName);
                    break;
                case "By Power":
                    list.ItemsSource = _listItems.OrderBy(c => c.Power);
                    break;
                case "By Odometer":
                    list.ItemsSource = _listItems.OrderBy(c => c.Odometer);
                    break;
            }
        }

        private void Clear_Button(object sender, System.EventArgs e)
        {
            list.ItemsSource = _listItems;
            search.Text = null;
        }

        private void User_Search_Car_Button(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                list.ItemsSource = _listItems;
            }
            else
                list.ItemsSource = _listItems.Where(x => x.FullName.StartsWith(e.NewTextValue));
        }

        async void ListItemTapped(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            var listItem = e.SelectedItem as CarsSoldListItem;
            
            var car = await App.CarManager.GetCar(listItem.CarId);

            if (car != null) { 
                    await Navigation.PushAsync(new CarSoldPage(car));
                    ((ListView)sender).SelectedItem = null;
            }
            else
            {
                ((ListView)sender).SelectedItem = null;
                await DisplayAlert("No internet connection", "It appears you have no internet connection.\nPlease try again.", "OK");

            }
        }
    }
}