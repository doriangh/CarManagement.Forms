using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestProiectLicenta.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class SellDetailsModalPage : ContentPage
    {
        private readonly Car _car;

        private CarVinRequest _carRequest;
        List<CarImages> images;

        public SellDetailsModalPage(Car usercar)
        {
            InitializeComponent();
            _car = usercar;
            BindingContext = _car;
            Task.WhenAll(PopulateCoverFlow());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _carRequest = new CarVinRequest
            {
                Car = new Car(),
                Errors = new List<string>()
            };

        }

        private async Task PopulateCoverFlow(bool force = false)
        {
            images = await App.CarImagesManager.GetCarsImages(_car.Id, force);
            carousel.ItemsSource = images;
            var userId = await SecureStorage.GetAsync("UserId");
            var user = await App.UserManager.GetUserById(Convert.ToInt32(userId), force);
            name.Text = user.Name;
            phone.Text = user.PhoneNumber.ToString();
            address.Text = user.Address;
        }

        private async void Cancel_Button(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Add_New_Image_Button(object sender, System.EventArgs e)
        {
            var action = await DisplayActionSheet("How would you like to add the image?", "Cancel", null, "Take Photo",
               "Select Photo");

            switch (action)
            {
                case "Take Photo":
                    {
                        _carRequest = await App.ExternalAPIManager.HandleTakingPicture(_carRequest);

                        if (_carRequest.Success)
                        {
                            var newImage = new CarImages
                            {
                                carId = _car.Id,
                                CarImage = _carRequest.Car.CarImage
                            };

                            //images.Add(_carRequest.Car.CarImage);
                            await App.CarImagesManager.AddCarImages(newImage);
                            await PopulateCoverFlow(true);
                        }
                        else
                            await DisplayAlert("Error", _carRequest.Errors[0], "OK");

                        break;
                    }

                case "Select Photo":
                    {
                        _carRequest = await App.ExternalAPIManager.HandleSelectionPicture(_carRequest);

                        if (_carRequest.Success)
                        {
                            var newImage = new CarImages
                            {
                                carId = _car.Id,
                                CarImage = _carRequest.Car.CarImage
                            };

                            //carImages.Add(_carRequest.Car.CarImage);
                            await App.CarImagesManager.AddCarImages(newImage);
                            await PopulateCoverFlow(true);
                        }
                        else
                            await DisplayAlert("Error", _carRequest.Errors[0], "OK");

                        break;
                    }
            }
        }

        private async void Sell_Car_Button(object sender, System.EventArgs e)
        {
            var userId = await SecureStorage.GetAsync("UserId");
            var carDetailsId = await App.CarDetailManager.GetCarsDetail(_car.Id);
            var doesExist = await App.CarsSoldManager.GetByCarId(_car.Id);

            if (_car.CarPrice != newPrice.Text && error.Text == "")
            {
                _car.CarPrice = newPrice.Text;
                await App.CarManager.UpdateCar(_car.Id, _car);
            }

            if (_car.CarPrice == null)
            {
                await DisplayAlert("No Price", "Please enter a price for car", "OK");
            }
            else
            {

                var soldCar = new CarsSold
                {
                    UserId = Convert.ToInt32(userId),
                    CarId = _car.Id,
                    CarDetail = carDetailsId.Id,
                    Details = description.Text,
                    LongDescription = longDescription.Text
                };


                if (doesExist == null)
                {
                    await App.CarsSoldManager.Add(soldCar);
                }
                else
                {
                    await App.CarsSoldManager.Update(soldCar);
                }
                await Navigation.PopToRootAsync();
            }
        }

        void Price_Changed(object sender, TextChangedEventArgs e)
        {
            if (_car.CarPrice != null)
                recommended.Text = string.Format("Recommended sale price is: {0}", _car.CarPrice);

            if (int.TryParse(e.NewTextValue, out int price))
            {
                if (price > 999999)
                    error.Text = "Please enter a smaller value";
                else if(price <= 0)
                    error.Text = "Your can't be worth just that";
                else
                    error.Text = "";
            }
            else
            {
                error.Text = "Please enter a numeric value";
            }
        }

  
    }
}
