using System;
using MonkeyCache.FileStore;
using Plugin.Fingerprint;
using TestProiectLicenta.Data.Services;
using TestProiectLicenta.Views;
using Xamarin.Forms;

namespace TestProiectLicenta
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();

            UserManager = new UserManager(new UserService());
            CarManager = new CarManager(new CarService());
            ExternalAPIManager = new ExternalApiManager(new ExternalApiService());
            CarDetailManager = new CarDetailManager(new CarDetailService());
            GetCarPriceManager = new GetCarPriceManager(new GetCarPriceService());
            CarImagesManager = new CarImagesManager(new CarImagesService());
            CarsSoldManager = new CarsSoldManager(new CarsSoldService());

            MainPage = new NavigationPage(new SplashScreenForms());

            Barrel.ApplicationId = "car_management";

            //MainPage = new NavigationPage(new UserLoginPage())
            //{
            //    BackgroundColor = Color.Black
            //};

            //MainPage = new NavigationPage(new CarListPage());
        }

        public static UserManager UserManager { get; set; }
        public static CarManager CarManager { get; set; }
        public static ExternalApiManager ExternalAPIManager { get; set; }
        public static CarDetailManager CarDetailManager { get; set; }
        public static GetCarPriceManager GetCarPriceManager { get; set; }
        public static CarImagesManager CarImagesManager { get; set; }
        public static CarsSoldManager CarsSoldManager { get; set; }

        protected override async void OnStart()
        {
            // Handle when your app starts
            var isLoggedIn = await UserManager.CheckLogIn();

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
                                MainPage = new NavigationPage(new UserPageForm());
                            }
                            else
                            {
                                MainPage = new NavigationPage(new UserLoginPage());
                            }
                        }
                    }
                    else
                    {
                        MainPage = new NavigationPage(new UserPageForm());
                    }
                }
                else
                {
                    Application.Current.Properties["FaceID"] = false;
                    MainPage = new NavigationPage(new UserPageForm());
                }
            }
        
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}