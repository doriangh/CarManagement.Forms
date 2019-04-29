﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Fingerprint;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class UserLoginPage : ContentPage
    {
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
    }
}
