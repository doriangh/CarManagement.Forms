using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
            Task.WhenAll(PopulateUserPage());
        }

        private async Task PopulateUserPage(bool force = false)
        {
            var userId = await SecureStorage.GetAsync("UserId");
            user = await App.UserManager.GetUserById(Convert.ToInt32(userId), force);
            cars = await App.CarManager.GetUserCars(Convert.ToInt32(userId), force);

            if (user.UserImage != null)
                topAvatar.Source = user.UserImage;

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
                    await PopulateUserPage(true);
                }
                else
                {
                    await Navigation.PushPopupAsync(new AddUserAddressPage(user));
                    await PopulateUserPage(true);
                }
            }
        }

        async void Add_Phone_Number_Button(object sender, System.EventArgs e)
        {
            await Navigation.PushPopupAsync(new AddPhoneNumberPage(user));
            await PopulateUserPage(true);
        }

        async void Add_New_User_Image(object sender, System.EventArgs e)
        {
            var result = await DisplayActionSheet("Choose a method", "Cancel", null, "Take Photo", "Select Photo");

            switch (result)
            {
                case "Take Photo":
                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await DisplayAlert("Error", "No camera available", "OK");
                        break;
                    }
                    var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg"
                    });

                    using (UserDialogs.Instance.Loading("Uploading image..."))
                    {

                        if (file == null)
                        {
                            await DisplayAlert("Error", "Could not save photo", "OK");
                            break;
                        }

                        var memoryStream = new MemoryStream();
                        file.GetStream().CopyTo(memoryStream);

                        user.UserImage = App.ExternalAPIManager.UploadImageImgur(file);

                        await App.UserManager.UpdateUser(user);

                        File.Delete(file.Path);
                    }
                    await DisplayAlert("Success", "Image updated successfully", "OK");
                    await PopulateUserPage(true);
                    break;

                case "Select Photo":
                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await DisplayAlert("Error", "Could not open gallery", "OK");
                        break;
                    }

                    using (UserDialogs.Instance.Loading("Uploading image..."))
                    {
                        file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions());

                        if (file == null)
                        {
                            await DisplayAlert("Error", "Could not read file", "OK");
                            break;
                        }

                        var memoryStream = new MemoryStream();
                        file.GetStream().CopyTo(memoryStream);

                        user.UserImage = App.ExternalAPIManager.UploadImageImgur(file);
                        await App.UserManager.UpdateUser(user);

                        File.Delete(file.Path);
                    }
                    await DisplayAlert("Success", "Image updated successfully", "OK");
                    await PopulateUserPage(true);
                    break;
            }
        }

        async void Change_Password_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushPopupAsync(new ChangePasswordPage(user));
        }
    }
}
