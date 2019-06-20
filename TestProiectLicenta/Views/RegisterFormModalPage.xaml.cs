using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Xml.Linq;
using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using TestProiectLicenta.Data.Services;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class RegisterFormModalPage : ContentPage
    {
        private readonly string _imgurId = "3998115b75eb6f3";
        private string _imgurSecret = "17246fb9c2e052d96773af41fdf5091b7ba71603";
        private UserService service = new UserService();

        private string userPhoto;

        public RegisterFormModalPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void RegisterUser(object sender, EventArgs e)
        {
            if (await App.UserManager.GetUserByUsername(user.Text, true) != null) return;

            var newUser = new User
            {
                Name = name.Text,
                Age = Convert.ToInt16(age.Text),
                Username = user.Text,
                Password = pass.Text,
                UserImage = userPhoto ?? null
            };

            await App.UserManager.AddUser(newUser);
            await App.UserManager.LogIn(newUser.Username, newUser.Password);
            Application.Current.Properties["FaceID"] = false;

            await Navigation.PopModalAsync();
        }

        private async void CancelButton(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void AddUserPhotoButton(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No camera", ":( No camera available", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg",
                DefaultCamera = CameraDevice.Front,
                RotateImage = false
            });

            if (file == null) return;

            //await DisplayAlert("File Location", file.Path, "OK");

            using (UserDialogs.Instance.Loading("Uploading Image"))
            {
                userPhoto = App.ExternalAPIManager.UploadImageImgur(file); 
            }

            //var memoryStream = new MemoryStream();
            //file.GetStream().CopyTo(memoryStream);


            //userPhoto = memoryStream.ToArray();

            File.Delete(file.Path);
        }
    }
}