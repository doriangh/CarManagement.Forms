using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Extensions;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class UserSelectedCarDetailPage : ContentPage
    {
        private readonly Car usercar;


        public UserSelectedCarDetailPage(Car car)
        {
            InitializeComponent();
            usercar = car;

            CarDetailLayout.BindingContext = usercar;

            if (usercar.Vin == null)
                addVin.IsEnabled = true;
            else
                addVin.IsEnabled = false;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (usercar.CarPrice == null)
            {
                priceLoading.IsRunning = true;
                priceLoading.IsVisible = true;

                if (CrossConnectivity.Current.IsConnected)
                {
                    var carPrice = await GetCarPrice(usercar);

                    if (carPrice.Errors != null && carPrice.Errors.Count > 0)
                    {
                        priceLoading.IsRunning = false;
                        priceLoading.IsVisible = false;
                        price.Text = "Could not estimate car price";
                        price.TextColor = Color.Red;
                    }
                    else
                    {
                        priceLoading.IsRunning = false;
                        priceLoading.IsVisible = false;
                        price.Text = carPrice.Price.ToString() + " EUR";
                    }
                }
                else
                {
                    priceLoading.IsRunning = false;
                    priceLoading.IsVisible = false;
                    price.Text = "No internet connection";
                    price.TextColor = Color.Red;
                }
            }
        }

        private async void EditCarButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UpdateCarPage(usercar.Id));
        }

        private async void AddVinButton(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new VinPopupPage(usercar));
            BindingContext = usercar;
        }

        private async void DeleteCarButton(object sender, EventArgs e)
        {
            var response = await DisplayAlert("Are you sure?", "Are you sure you want to delete this car?", "Yes", "No");

            if (response)
            {
                using (UserDialogs.Instance.Loading($"Removing car./nPlease wait!"))
                {
                    await App.CarManager.DeleteCar(usercar.Id);
                    await Navigation.PopModalAsync();
                }
            }
        }

        private static async Task<CarPriceResponse> GetCarPrice(Car car)
        {
            var priceDetails = new CarPriceRequest
            {
                Model = car.Model.Replace(" ", ""),
                Make = car.Make.Replace(" ", ""),
                Cc = Convert.ToInt32(car.Cc),
                Odometer = Convert.ToInt32(car.Odometer),
                Year = Convert.ToInt32(car.ModelYear),
                Power = Convert.ToInt32(car.Power),
                Fuel = car.Fuel
            };

            var carPrice = await App.GetCarPriceManager.GetCarPrice(priceDetails);
            return carPrice;
        }

        async void CloseModalButton(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Sell_Car_Button(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new SellDetailsModalPage(usercar)));
        }
    }
}