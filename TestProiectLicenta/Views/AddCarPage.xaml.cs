using System;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class AddCarPage : ContentPage
    {
        public AddCarPage()
        {
            InitializeComponent();
        }

        private async void BackButtonPressed(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void TakePictureButtonPressed(object sender, EventArgs e)
        {
            var request = await App.ExternalAPIManager.GetCarByTakingPictureAsync();

            if (request.Success)
            {
                var action = await DisplayAlert("Is this your car?",
                    $"Car Make: {request.Car.Make}\nCar Model: {request.Car.Model}\nCar Color: {request.Car.Color}\nCar Type: {request.Car.Body}\n Car Licese: {request.Car.License}",
                    "Yes", "No");

                if (action)
                    //await App.CarManager.AddCar(request.Car);

                    await Navigation.PushAsync(new CarDetailsSetupFormPage(request));
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
                    //await App.CarManager.AddCar(request.Car);

                    await Navigation.PushAsync(new CarDetailsSetupFormPage(request));
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

        private async void AddManuallyButtonPressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CarDetailsSetupFormPage());
        }

        private async void AddVinButtonPressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCarVinPage());
        }
    }
}