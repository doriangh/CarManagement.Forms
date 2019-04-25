using TestProiectLicenta.Data.Services;
using TestProiectLicenta.Persistence;
using Xamarin.Forms;

namespace TestProiectLicenta
{
    public partial class App : Application
    {
        public static UserManager UserManager { get; set; }
        public static CarManager CarManager { get; set; }
        public static ExternalAPIManager ExternalAPIManager { get; set; }

        public App()
        {
            InitializeComponent();

            UserManager = new UserManager(new UserService());
            CarManager = new CarManager(new CarService());
            ExternalAPIManager = new ExternalAPIManager(new ExternalAPIService());

            MainPage = new NavigationPage(new UserLoginPage())
            {
                BarBackgroundColor = Color.Black
            };
        }

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
