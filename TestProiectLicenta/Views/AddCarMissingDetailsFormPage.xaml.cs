using System;
using System.Collections.Generic;
using System.Reflection;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class AddCarMissingDetailsFormPage : ContentPage
    {
        CarVinRequest _request;

        public AddCarMissingDetailsFormPage(CarVinRequest request)
        {
            InitializeComponent();

            _request = request;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            PropertyInfo[] properties = _request.Car.GetType().GetProperties();
            foreach (var item in properties)
            {
                if (item.GetValue(_request.Car, null) is null)
                {
                    missing.Add(new EntryCell { Text = item.Name, Placeholder = "eg. " + item.Name });
                }
                continue;
            }
        }
    }
}
