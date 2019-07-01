using System;
using System.Security.Cryptography;
using System.Text;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Views
{
    public partial class ChangePasswordPage : PopupPage
    {
        private User _user;

        public ChangePasswordPage(User user)
        {
            InitializeComponent();

            _user = user;
        }

        async void Cancel_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        async void Update_Clicked(object sender, System.EventArgs e)
        {
            if (newPassword.Text != newRePassword.Text)
            {
                error.Text = "Please enter the same password in both fields!";
                return;
            }

            if (newPassword.Text.Length < 8)
            {
                error.Text = "The password has to be 8 characters or more";
                return;
            }

            if (Encrypt(currPass.Text) != _user.Password)
            {
                error.Text = "Current password incorrect";
                return;
            }

            _user.Password = Encrypt(newPassword.Text);
            await App.UserManager.UpdateUser(_user);
            await Navigation.PopPopupAsync();
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            error.Text = null;
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
