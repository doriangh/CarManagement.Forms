using System;
using System.Collections.Generic;
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

            UserDialogs.Instance.ShowLoading("Please wait");
            var isLoggedIn = await App.UserManager.CheckLogIn();
            UserDialogs.Instance.Loading().Dispose();

            if (isLoggedIn)
            {
                await Navigation.PushAsync(new UserPageForm());

                //if (await CrossFingerprint.Current.IsAvailableAsync(allowAlternativeAuthentication:true))
                //{
                //    var result = await CrossFingerprint.Current.AuthenticateAsync("Prove you have fingers");

                //    if (result.Authenticated)
                //    {
                //        await Navigation.PushAsync(new UserPageForm());
                //    }
                //}
            }
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
    }
}
