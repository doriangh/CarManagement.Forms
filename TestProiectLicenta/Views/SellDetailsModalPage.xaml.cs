using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using TestProiectLicenta.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class SellDetailsModalPage : ContentPage
    {
        private Car car;

        CarVinRequest _carRequest;
        List<CarImages> images;

        public SellDetailsModalPage(Car usercar)
        {
            InitializeComponent();
            car = usercar;
            BindingContext = car;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            _carRequest = new CarVinRequest
            {
                Car = new Car(),
                Errors = new List<string>()
            };

            await PopulateCoverFlow();
        }

        async Task PopulateCoverFlow()
        {
            images = await App.CarImagesManager.GetCarsImages(car.Id);
            carousel.ItemsSource = images;
        }

        async void Cancel_Button(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Add_New_Image_Button(object sender, System.EventArgs e)
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
                                carId = car.Id,
                                CarImage = _carRequest.Car.CarImage
                            };

                            //images.Add(_carRequest.Car.CarImage);
                            App.CarImagesManager.AddCarImages(newImage);
                            await PopulateCoverFlow();
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
                                carId = car.Id,
                                CarImage = _carRequest.Car.CarImage
                            };

                            //carImages.Add(_carRequest.Car.CarImage);
                            App.CarImagesManager.AddCarImages(newImage);
                            await PopulateCoverFlow();
                        }
                        else
                            await DisplayAlert("Error", _carRequest.Errors[0], "OK");

                        break;
                    }
            }
        }

        async void Sell_Car_Button(object sender, System.EventArgs e)
        {
            var userId = await SecureStorage.GetAsync("UserId");
            var carDetailsId = await App.CarDetailManager.GetCarsDetail(car.Id);

            var soldCar = new CarsSold
            {
                UserId = Convert.ToInt32(userId),
                CarId = car.Id,
                CarDetails = carDetailsId.Id,
                Details = description.Text,
                LongDescription = longDescription.Text
            };

            await App.CarsSoldManager.Add(soldCar);
            await Navigation.PopToRootAsync();
        }

        async void Change_Price(object sender, System.EventArgs e)
        {
            int newPrice;

            await UserDialogs.Instance.PromptAsync(new PromptConfig
            {
                Placeholder = "Enter text",
                Title = "Please enter a new value",
                IsCancellable = true,
                OnTextChanged = args =>
                {
                    newPrice = Convert.ToInt32(args);
                }
            });
        }
    }
}
