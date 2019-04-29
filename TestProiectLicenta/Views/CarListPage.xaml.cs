using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class CarListPage : ContentPage
    {
        public CarListPage()
        {
            InitializeComponent();
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            var userId = await SecureStorage.GetAsync("UserId");

            var cars = await App.CarManager.GetUserCars(Convert.ToInt32(userId));

            list.ItemsSource = cars;
            list.RowHeight = 200;
        }

        void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}