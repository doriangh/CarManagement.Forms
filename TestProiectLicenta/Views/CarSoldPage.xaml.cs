using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Messaging;
using TestProiectLicenta.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TestProiectLicenta.Views
{
    public partial class CarSoldPage : ContentPage
    {
        private Car _car;
        private List<CarImages> images;
        private List<CarsSold> soldCar;
        private CarsSold currentCar;
        private User user;

        public CarSoldPage(Car car)
        {
            InitializeComponent();
            _car = car;
            //Task.WaitAll(PopulateCoverFlow());
            Task.WhenAll(PopulateCoverFlow());
        }

        private async Task PopulateCoverFlow(bool force = false)
        {
            images = await App.CarImagesManager.GetCarsImages(_car.Id, force);
            soldCar = await App.CarsSoldManager.GetAll(force);
            currentCar = soldCar.FirstOrDefault(x => x.CarId == _car.Id);
            description.Text = currentCar.LongDescription;
            carousel.ItemsSource = images;

            BindingContext = _car;

            user = await App.UserManager.GetUserById(currentCar.UserId);

            var locations = await Geocoding.GetLocationsAsync(user.Address);
            var location = locations?.FirstOrDefault();
            if (location != null)
            {
                map.MoveToRegion(
                    MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMeters(500)));

                map.IsVisible = true;
            }
            var loggedUser = await SecureStorage.GetAsync("UserId");
            if (Convert.ToInt32(loggedUser) == user.Id)
            {
                ToolbarItems.Add(new ToolbarItem("Edit", "edit_car_toolbar", () =>
                {
                    Navigation.PushModalAsync(new SellDetailsModalPage(_car));
                }));

                ToolbarItems.Add(new ToolbarItem("Delete", "delete_car_toolbar", async () =>
                {
                    var alert = await DisplayAlert("Are you sure?", "Are you sure you want to delete your post? You can always create a new one by going to your cars", "Yes", "No");

                    if (alert)
                    {
                        await App.CarsSoldManager.Delete(currentCar.id);
                        await Navigation.PopAsync();
                    }

                }));
            }
        }

        void Call_Seller(object sender, System.EventArgs e)
        {
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            if (phoneDialer.CanMakePhoneCall)
                phoneDialer.MakePhoneCall("0" + user.PhoneNumber.ToString());
        }
    }
}
