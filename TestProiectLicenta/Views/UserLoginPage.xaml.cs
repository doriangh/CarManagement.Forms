using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Fingerprint;
using SQLite;
using TestProiectLicenta.Interfaces.Services;
using TestProiectLicenta.Models;
using TestProiectLicenta.Persistence;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta
{
    public partial class UserLoginPage : ContentPage
    {
        public int UserId;

        public UserLoginPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await CheckIfLoggedIn();
        }

        async void LoginButton(object sender, System.EventArgs e)
        {

            var loginForm = new LoginFormModalPage();

            await Navigation.PushModalAsync(loginForm);
        }

        async void RegisterButton(object sender, System.EventArgs e)
        {
            var registerPage = new RegisterFormModalPage();

            await Navigation.PushModalAsync(registerPage);
        }

        async void TryAgainButton(object sender, System.EventArgs e)
        {
            await CheckIfLoggedIn();
        }

        async Task CheckIfLoggedIn()
        {
            bool isLoggedIn;

            using (UserDialogs.Instance.Loading("Please wait"))
            {
                isLoggedIn = await App.UserManager.CheckLogIn();
            }

            if (isLoggedIn)
            {
                if (await CrossFingerprint.Current.IsAvailableAsync(true))
                {
                    var result = await CrossFingerprint.Current.AuthenticateAsync("Prove you have fingers");

                    if (result.Authenticated)
                    {
                        Debug.WriteLine(result.ErrorMessage);
                        Debug.WriteLine(result.Status);
                        retry.IsVisible = false;
                        await Navigation.PushAsync(new UserPageForm());
                    }
                    else
                    {
                        await DisplayAlert("Authentication Error", "Could not authenticate", "OK");
                        retry.IsVisible = true;
                    }
                }
            }
        }


    }
}
