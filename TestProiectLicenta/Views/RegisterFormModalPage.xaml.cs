using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
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
        private readonly string _imgurSecret = "17246fb9c2e052d96773af41fdf5091b7ba71603";
        private readonly UserService service = new UserService();

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
            if (await App.UserManager.GetUserByUsername(user.Text, true) != null)
            {
                await DisplayAlert("User already exists", "The username you have entered already exists. Please try again.", "OK");
                return;
            }

            if (pass.Text != rePass.Text)
            {
                await DisplayAlert("Incorrect password", "Please enter the same password in both fields.", "OK");
                return;
            }

            if (pass.Text.Length < 8)
            {
                await DisplayAlert("Password too short", "Please enter a password with a minimum of 8 characters.", "OK");
                return;
            }

            var verifyName = new Regex(@"^[a-zA-Z ]+$", RegexOptions.Compiled);
            if (!verifyName.IsMatch(name.Text))
            {
                await DisplayAlert("Incorrect name", "That might not be a real name.", "OK");
                return;
            }

            var verifyNumber = new Regex(@"^[1-9][0-9]+$", RegexOptions.Compiled);
            if (!verifyNumber.IsMatch(age.Text))
            {

                await DisplayAlert("Age incorrect", "Your age should only have numbers", "OK");

            }
            else
            {
                if (Convert.ToInt32(age.Text.Length) > 2)
                {
                    await DisplayAlert("Age incorrect", "You might be old, but not that old.", "OK");
                    return;
                }
            }


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
                RotateImage = true
            });

            if (file == null) return;

            //await DisplayAlert("File Location", file.Path, "OK");

            using (UserDialogs.Instance.Loading("Uploading Image"))
            {
                userPhoto = App.ExternalAPIManager.UploadImageImgur(file);

                //var memoryStream = new MemoryStream();
                //file.GetStream().CopyTo(memoryStream);


                //userPhoto = memoryStream.ToArray();

                File.Delete(file.Path);
            }
        }
    }
}