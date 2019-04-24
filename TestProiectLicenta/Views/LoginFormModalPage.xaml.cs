using System;
using System.Security.Cryptography;
using System.Text;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta
{
    public partial class LoginFormModalPage : ContentPage
    {
        User userObj;

        public LoginFormModalPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        async void UserLogInButton(object sender, System.EventArgs e)
        {
            if (user.Text != null && pass.Text != null)
            {
                var username = user.Text;
                var password = Encrypt(pass.Text);

                userObj = await App.UserManager.GetUserByUsername(username);

                if (userObj is null)
                {
                    message.Text = "User not found!";
                    message.IsVisible = true;
                    return;
                }
        
                if(userObj.Password == password)
                {
                    Console.WriteLine(username + ' ' + pass.Text + ' ' + password);

                    await App.UserManager.LogIn(userObj.Username, userObj.Password);

                    //await SecureStorage.SetAsync("session_key", session.Key);
                    await Navigation.PopModalAsync();
                }
                else
                {
                    message.Text = "Password incorrect";
                    message.IsVisible = true;
                    return;
                }

            }
            else
            {
                message.IsVisible = true;
            }
        }

        async void CancelButton(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        static string Encrypt(string value)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(value));
                return Convert.ToBase64String(data);
            }
        }
    }
}
