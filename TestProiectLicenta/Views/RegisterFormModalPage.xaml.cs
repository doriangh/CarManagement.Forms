using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Xml.Linq;
using Plugin.Media;
using TestProiectLicenta.Data.Services;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta
{
    public partial class RegisterFormModalPage : ContentPage
    {

        string imgurId = "3998115b75eb6f3";
        string imgurSecret = "17246fb9c2e052d96773af41fdf5091b7ba71603";
        UserService service = new UserService();

        private string userPhoto;

        public RegisterFormModalPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void RegisterUser(object sender, System.EventArgs e)
        {
            if (await App.UserManager.GetUserByUsername(user.Text) != null){
                return;
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

        private async void CancelButton(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void AddUserPhotoButton(object sender, System.EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No camera", ":( No camera available", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg",
                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
                RotateImage = false
            });

            if (file == null)
            {
                return;
            }

            await DisplayAlert("File Location", file.Path, "OK");

            using (var w = new WebClient())
            {
                w.Headers.Add("Authorization", "Client-ID " + imgurId);
                var values = new NameValueCollection
                {
                    {"image", Convert.ToBase64String(File.ReadAllBytes(file.Path))}
                };

                var response = w.UploadValues("https://api.imgur.com/3/upload.xml", values);
                var xml = XDocument.Load(new MemoryStream(response));
                Console.WriteLine(xml);

                await DisplayAlert("Link", "Link is here: " + xml.Root.Element("link").Value, "OK");

                userPhoto = xml.Root.Element("link").Value;
            }

            //var memoryStream = new MemoryStream();
            //file.GetStream().CopyTo(memoryStream);




            //userPhoto = memoryStream.ToArray();

            File.Delete(file.Path);

        }
    }
}
