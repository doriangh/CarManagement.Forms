using System;
using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class ChooseMethodFormPage : ContentPage
    {
        public ChooseMethodFormPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void Add_Manually_Button(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new FillExtraFormPage());
        }

        private async void Add_VIN_Button(object sender, System.EventArgs e)
        {
            await Navigation.PushPopupAsync(new VinPopupPage());
        }

        private async void TakePictureButtonPressed(object sender, EventArgs e)
        {
            CarVinRequest request;

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Error", "Cannot take picture.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            using (UserDialogs.Instance.Loading("Finding that car"))
            {
                request = await App.ExternalAPIManager.GetCarByTakingPictureAsync(file);
            }

            if (request.Success)
            {
                var action = await DisplayAlert("Is this your car?",
                    $"Car Make: {request.Car.Make}\nCar Model: {request.Car.Model}\nCar Color: {request.Car.Color}\nCar Type: {request.Car.Body}\n Car Licese: {request.Car.License}",
                    "Yes", "No");

                if (action)

                    await Navigation.PushAsync(new FillExtraFormPage(request));
                else
                    await DisplayAlert("Sorry",
                        "Please try again. For the best results, make sure there is enough light, the License plate is clearly visible, and your car takes up most of the image.",
                        "OK");
            }
            else
            {
                await DisplayAlert("Error", request.Errors[0], "OK");
            }
        }

        private async void AddPictureButtonPressed(object sender, EventArgs e)
        {
            var request = await App.ExternalAPIManager.GetCarBySelectingPicture();

            if (request.Success)
            {
                var action = await DisplayAlert("Is this your car?",
                    $"Car Make: {request.Car.Make}\nCar Model: {request.Car.Model}\nCar Color: {request.Car.Color}\nCar Type: {request.Car.Body}\n Car License: {request.Car.License}",
                    "Yes", "No");

                if (action)
                {
                    await Navigation.PushAsync(new FillExtraFormPage(request));
                }
                else
                    await DisplayAlert("Sorry",
                        "Please try again. For the best results, make sure there is enough light, the License plate is clearly visible, and your car takes up most of the image.",
                        "OK");
            }
            else
            {
                await DisplayAlert("Error", request.Errors[0], "OK");
            }
        }
    }
}

