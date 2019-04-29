using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class AddCarVINPage : ContentPage
    {
        public AddCarVINPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        async void FindCarVINButton(object sender, System.EventArgs e)
        {
            string VIN = VINField.Text;

            var data = await App.ExternalAPIManager.GetCarByVIN(VIN);

            await App.CarManager.AddCarByVIN(data, VIN);

            await Navigation.PushAsync(new UserPageForm());
        }
    }
}
