using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using TestProiectLicenta.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TestProiectLicenta.Views
{
    public partial class UserViewPage : ContentPage
    {
        User user;
        List<Car> cars;

        public UserViewPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var userId = await SecureStorage.GetAsync("UserId");
            user = await App.UserManager.GetUserById(Convert.ToInt32(userId));
            cars = await App.CarManager.GetUserCars(Convert.ToInt32(userId));

            topAvatar.Source = user.UserImage;
            //username.Text = user.Name;

            BindingContext = user;

            carCount.Text = cars.Count.ToString();

            var value = 0;

            foreach (var car in cars)
                value += Convert.ToInt32(car.CarPrice);

            carAmount.Text = value.ToString();

        }

        async void Add_Address_Button(object sender, System.EventArgs e)
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                Geocoder geocoder = new Geocoder();
                List<string> addresslist = new List<string>();

                var position = new Position(location.Latitude, location.Longitude);
                var addresses = await geocoder.GetAddressesForPositionAsync(position);
                foreach (var address in addresses)
                {
                    addresslist.Add(address);
                }

                var x = await DisplayAlert("Is this your location", addresslist[0], "Yes", "No");

                if (x)
                {
                    user.Address = addresslist[0];
                    await App.UserManager.UpdateUser(user);
                }
                else
                {
                    await Navigation.PushPopupAsync(new AddUserAddressPage(user));
                }
            }
        }

        void Add_Phone_Number_Button(object sender, System.EventArgs e)
        {
            
        }
    }
}
