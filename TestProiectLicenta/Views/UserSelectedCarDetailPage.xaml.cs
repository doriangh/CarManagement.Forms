using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using Rg.Plugins.Popup.Extensions;
using TestProiectLicenta.Models;
using TestProiectLicenta.Views;
using Xamarin.Forms;

namespace TestProiectLicenta
{
    public partial class UserSelectedCarDetailPage : CarouselPage
    { 
        private Car usercar;


        public UserSelectedCarDetailPage(Car car)
        {
            InitializeComponent();
            usercar = car;

            //var user = new PexelsClient("563492ad6f9170000100000100b4df8ef66f4f218c4eb57e60ac194d");

            //var results = user.SearchAsync(usercar.FullName + usercar.ModelYear).Result;

            //CarDetailLayout.Children.Add(new Image { Source = ImageSource.FromUri(new Uri(results.Photos[0].Src.Original)) });

            if (usercar.CarImage == null)
            {
                var url = "http://www.carimagery.com/api.asmx/GetImageUrl?searchTerm=" + usercar.Make + "+" + usercar.Model + ("+" + usercar.ModelYear) ?? null + ("+" + usercar.Body) ?? null;

                var reader = new XmlTextReader(url);

                while (reader.Read())
                {
                    if (reader.Value.Contains("http://"))
                    {
                        carImage.Source = ImageSource.FromUri(new Uri(reader.Value.Trim()));
                    }
                    //Console.WriteLine(reader.Value.Trim());

                }
            }
            else
            {
                carImage.Source = usercar.CarImage;
            }

            if (usercar.Vin == null)
                addVin.IsVisible = true;



            CarDetailLayout.BindingContext = usercar;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var carPrice = await GetCarPrice(usercar);

            if (carPrice.Errors != null)
            {
                price.Text = "Could not estimate car price;";
                price.TextColor = Color.Red;
            }
            else
            {
                price.Text = carPrice.Price.ToString();
                estimation.Text = "Value obtained from " + carPrice.Count + " cars queried.";
            }
        }

        private async void EditCarButton(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new UpdateCarPage(usercar.Id));
        }

        private async void AddVinButton(object sender, System.EventArgs e)
        {
            var result = await Plugin.DialogKit.CrossDiaglogKit.Current.GetInputTextAsync("Enter VIN", "Please enter VIN");

            if (result != null)
            {
                usercar.Vin = result;

                await App.CarManager.UpdateCar(usercar.Id, usercar);
            }

            BindingContext = usercar;
        }

        private async void DeleteCarButton(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private static async Task<CarPriceResponse> GetCarPrice(Car car)
        {
            var priceDetails = new CarPriceRequest()
            {
                Model = car.Model,
                Make = car.Make,
                Cc = Convert.ToInt32(car.Cc),
                Odometer = Convert.ToInt32(car.Odometer),
                Year = Convert.ToInt32(car.ModelYear)
            };

            var carPrice = await App.GetCarPriceManager.GetCarPrice(priceDetails);
            return carPrice;

        }
    }
}
