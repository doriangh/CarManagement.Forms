using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Fingerprint;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class UserLoginPage : ContentPage
    {
        private User _userObj;
        public int UserId;

        public UserLoginPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await CheckIfLoggedIn();
        }

        private async void RegisterButton(object sender, EventArgs e)
        {
            var registerPage = new RegisterFormModalPage();

            await Navigation.PushModalAsync(registerPage);
        }

        private async void TryAgainButton(object sender, EventArgs e)
        {
            await CheckIfLoggedIn();
        }

        private async Task CheckIfLoggedIn()
        {
            bool isLoggedIn;

            using (UserDialogs.Instance.Loading("Please wait"))
            {
                isLoggedIn = await App.UserManager.CheckLogIn();
            }

            if (isLoggedIn)
            {
                if (Application.Current.Properties.ContainsKey("FaceID"))
                {
                    if (Convert.ToBoolean(Application.Current.Properties["FaceID"].ToString()))
                    {
                        if (await CrossFingerprint.Current.IsAvailableAsync(true))
                        {
                            var result = await CrossFingerprint.Current.AuthenticateAsync("Prove you have fingers");

                            if (result.Authenticated)
                            {
                                Debug.WriteLine(result.ErrorMessage);
                                Debug.WriteLine(result.Status);
                                Retry.IsVisible = false;
                                await Navigation.PushAsync(new UserPageForm());
                            }
                            else
                            {
                                await DisplayAlert("Authentication Error", "Could not authenticate", "OK");
                                Retry.IsVisible = true;
                            }
                        }
                    }
                    else
                    {
                        await Navigation.PushAsync(new UserPageForm());
                    }
                }
                else
                {
                    Application.Current.Properties["FaceID"] = false;
                    await Navigation.PushAsync(new UserPageForm());
                }
            }
        }

        private async void UserLogInButton(object sender, EventArgs e)
        {
            if (User.Text != null && Pass.Text != null)
            {
                var username = User.Text;
                var password = Encrypt(Pass.Text);

                _userObj = await App.UserManager.GetUserByUsername(username);

                if (_userObj is null)
                {
                    Message.Text = "User not found!";
                    Message.IsVisible = true;
                    return;
                }

                if (_userObj.Password == password)
                {
                    using (UserDialogs.Instance.Loading("Logging you in.\nHold on"))
                    {
                        await App.UserManager.LogIn(_userObj.Username, _userObj.Password);
                    }

                    await Navigation.PushAsync(new UserPageForm());
                }
                else
                {
                    Message.Text = "Password incorrect";
                    Message.IsVisible = true;
                }
            }
            else
            {
                Message.IsVisible = true;
            }
        }

        private async void CancelButton(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private static string Encrypt(string value)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var utf8 = new UTF8Encoding();
                var data = md5.ComputeHash(utf8.GetBytes(value));
                return Convert.ToBase64String(data);
            }
        }
    }
}