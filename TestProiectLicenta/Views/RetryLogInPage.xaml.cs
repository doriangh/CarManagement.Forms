using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Acr.UserDialogs;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class RetryLogInPage : PopupPage
    {
        public RetryLogInPage()
        {
            InitializeComponent();
        }

        async void Cancel_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        async void Submit_Clicked(object sender, System.EventArgs e)
        {
            var userId = await SecureStorage.GetAsync("UserId");
            var user = await App.UserManager.GetUserById(Convert.ToInt32(userId));

            if (Encrypt(pass.Text) == user.Password)
            {

            }
            
        }

        void Retry_Clicked(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
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
