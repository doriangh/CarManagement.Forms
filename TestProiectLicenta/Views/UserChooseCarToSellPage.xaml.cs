﻿using System;
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
    }
}
