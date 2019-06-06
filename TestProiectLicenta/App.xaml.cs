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

            MainPage = new NavigationPage(new UserLoginPage())
            {
                BackgroundColor = Color.Black
            };

            //MainPage = new NavigationPage(new CarListPage());
        }

        public static UserManager UserManager { get; set; }
        public static CarManager CarManager { get; set; }
        public static ExternalApiManager ExternalAPIManager { get; set; }
        public static CarDetailManager CarDetailManager { get; set; }
        public static GetCarPriceManager GetCarPriceManager { get; set; }

        protected override void OnStart()
        {
            // Handle when your app starts
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