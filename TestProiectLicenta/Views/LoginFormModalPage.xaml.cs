using System;
using System.Security.Cryptography;
using System.Text;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta
{
    public partial class LoginFormModalPage : ContentPage
    {
        private User _userObj;

        public LoginFormModalPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void UserLogInButton(object sender, System.EventArgs e)
        {
            if (user.Text != null && pass.Text != null)
            {
                var username = user.Text;
                var password = Encrypt(pass.Text);

                _userObj = await App.UserManager.GetUserByUsername(username);

                if (_userObj is null)
                {
                    message.Text = "User not found!";
                    message.IsVisible = true;
                    return;
                }
        
                if(_userObj.Password == password)
                {
                    Console.WriteLine(username + ' ' + pass.Text + ' ' + password);

                    await App.UserManager.LogIn(_userObj.Username, _userObj.Password);

                    //await SecureStorage.SetAsync("session_key", session.Key);
                    await Navigation.PopModalAsync();
                }
                else
                {
                    message.Text = "Password incorrect";
                    message.IsVisible = true;
                }

            }
            else
            {
                message.IsVisible = true;
            }
        }

        private async void CancelButton(object sender, System.EventArgs e)
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
