using System;
using System.Threading.Tasks;
using System.Xml;
using Plugin.DialogKit;
using Rg.Plugins.Popup.Extensions;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class UserSelectedCarDetailPage : CarouselPage
    {
        private readonly Car usercar;


        public UserSelectedCarDetailPage(Car car)
        {
            InitializeComponent();
            usercar = car;

            //var user = new PexelsClient("563492ad6f9170000100000100b4df8ef66f4f218c4eb57e60ac194d");

            //var results = user.SearchAsync(usercar.FullName + usercar.ModelYear).Result;

            //CarDetailLayout.Children.Add(new Image { Source = ImageSource.FromUri(new Uri(results.Photos[0].Src.Original)) });

            if (usercar.CarImage == null)
            {
                var url = "http://www.carimagery.com/api.asmx/GetImageUrl?searchTerm=" + usercar.Make + "+" +
                          usercar.Model + "+" + usercar.ModelYear ?? null + "+" + usercar.Body ?? null;

                var reader = new XmlTextReader(url);

                while (reader.Read())
                    if (reader.Value.Contains("http://"))
                        carImage.Source = ImageSource.FromUri(new Uri(reader.Value.Trim()));
                //Console.WriteLine(reader.Value.Trim());
            }
            else
            {
                carImage.Source = usercar.CarImage;
            }

            if (usercar.Vin == null)
                addVin.IsEnabled = true;
            else
                addVin.IsEnabled = false;                   


            CarDetailLayout.BindingContext = usercar;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (usercar.CarPrice == null)
            {
                var carPrice = await GetCarPrice(usercar);

                if (carPrice.Errors != null && carPrice.Errors.Count > 0)
                {
                    price.Text = "Could not estimate car price";
                    price.TextColor = Color.Red;
                }
                else
                {
                    price.Text = carPrice.Price.ToString() + "EUR";
                }
            }
            else
                price.Text = usercar.CarPrice + "EUR"; 
        }

        private async void EditCarButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UpdateCarPage(usercar.Id));
        }

        private async void AddVinButton(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new VinPopupPage(usercar));

            //var result = await CrossDiaglogKit.Current.GetInputTextAsync("Enter VIN", "Please enter VIN");

            //if (result != null)
            //{
            //    usercar.Vin = result;

            //    await App.CarManager.UpdateCar(usercar.Id, usercar);
            //}

            BindingContext = usercar;
        }

        private async void DeleteCarButton(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private static async Task<CarPriceResponse> GetCarPrice(Car car)
        {
            var priceDetails = new CarPriceRequest
            {
                Model = car.Model.Replace(" ", ""),
                Make = car.Make.Replace(" ",""),
                Cc = Convert.ToInt32(car.Cc),
                Odometer = Convert.ToInt32(car.Odometer),
                Year = Convert.ToInt32(car.ModelYear)
            };

            var carPrice = await App.GetCarPriceManager.GetCarPrice(priceDetails);
            return carPrice;
        }
    }
}