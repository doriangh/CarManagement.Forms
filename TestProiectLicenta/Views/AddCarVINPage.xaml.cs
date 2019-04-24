using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using SQLite;
using TestProiectLicenta.Logic;
using TestProiectLicenta.Models;
using TestProiectLicenta.Persistence;
using Xamarin.Forms;

namespace TestProiectLicenta
{
    public partial class AddCarVINPage : ContentPage
    {
        public AddCarVINPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        async void FindCarVINButton(object sender, System.EventArgs e)
        {
            string VIN = VINField.Text;

            var data = await App.ExternalAPIManager.GetCarByVIN(VIN);

            await App.CarManager.AddCarByVIN(data, VIN);

            await Navigation.PushAsync(new UserPageForm());
        }
    }
}
