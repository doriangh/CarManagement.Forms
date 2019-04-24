using System;
using Xamarin.Forms;

namespace TestProiectLicenta
{
    public partial class AddCarPage : ContentPage
    {
        public AddCarPage()
        {
            InitializeComponent();
        }

        async void BackButtonPressed(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void TakePictureButtonPressed(object sender, System.EventArgs e)
        {
            var request = await App.ExternalAPIManager.GetCarByTakingPictureAsync();

            if (request.Success) { 

                var action = await DisplayAlert("Is this your car?", String.Format("Car Make: {0}\nCar Model: {1}\nCar Color: {2}\nCar Type: {3}\n Car Licese: {4}", request.Car.Make, request.Car.Model, request.Car.Color, request.Car.Body, request.Car.License), "Yes", "No");

                if (action)
                {
                    await App.CarManager.AddCar(request.Car);

                    await Navigation.PushAsync(new UserPageForm());
                }
                else
                {
                    await DisplayAlert("Sorry", "Please try again. For the best results, make sure there is enough light, the License plate is clearly visible, and your car takes up most of the image.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", request.Errors[0], "OK");
            }
        }

        async void AddPictureButtonPressed(object sender, System.EventArgs e)
        {
            var request = await App.ExternalAPIManager.GetCarBySelectingPicture();

            if (request.Success)
            {
                var action = await DisplayAlert("Is this your car?", String.Format("Car Make: {0}\nCar Model: {1}\nCar Color: {2}\nCar Type: {3}\n Car Licese: {4}", request.Car.Make, request.Car.Model, request.Car.Color, request.Car.Body, request.Car.License), "Yes", "No");

                if (action)
                {
                    await App.CarManager.AddCar(request.Car);

                    await Navigation.PushAsync(new UserPageForm());
                }
                else
                {
                    await DisplayAlert("Sorry", "Please try again. For the best results, make sure there is enough light, the License plate is clearly visible, and your car takes up most of the image.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", request.Errors[0], "OK");
            }
        }

        public async void AddManuallyButtonPressed(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddCarFormPage());
        }

        async void AddVINButtonPressed(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddCarVINPage());
        }
    }
}
