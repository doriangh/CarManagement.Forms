using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Acr.UserDialogs;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using SQLite;
using TestProiectLicenta.Models;
using TestProiectLicenta.Persistence;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta
{
    public partial class UserPageForm : TabbedPage
    {
        public string userId;

        ObservableCollection<Car> userCars;

        public UserPageForm()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            UserDialogs.Instance.Toast("Getting user details.\nHold on");

            userId = await SecureStorage.GetAsync("UserId");

            var user = await App.UserManager.GetUserById(Convert.ToInt32(userId));


            name.Text = user.Name;
            if (user.UserImage != null && user.UserImage.Contains("https://"))
            {
                avatar.Source = ImageSource.FromUri(new Uri(user.UserImage));
            }

            userCars = new ObservableCollection<Car>();

            var lst = await App.CarManager.GetUserCars(user.Id);

            foreach (var car in lst)
            {
                userCars.Add(car);
            }

            if (userCars.Count > 0)
            {
                cars.IsVisible = true;
                addCar.IsVisible = false;
                cars.ItemsSource = userCars;
            }

            //UserDialogs.Instance.Loading().Dispose();
        }

        async void RefreshCars()
        {
           userCars.Clear();

            var lst = await App.CarManager.GetUserCars(Convert.ToInt32(userId));

            foreach (var car in lst)
            {
                userCars.Add(car);
                cars.IsVisible = true;
                addCar.IsVisible = false;
                cars.ItemsSource = userCars;
            }
        }

        async void AddCarsWhenNoCarsButton(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddCarPage());
        }

        async void BackButtonPressed(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public async void AddManuallyButtonPressed(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddCarFormPage());
        }

        async void AddVINButtonPressed(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddCarVINPage());
        }

        async void SignOutButton(object sender, System.EventArgs e)
        {
            SecureStorage.RemoveAll();

            await Navigation.PopToRootAsync();
        }

        async void ViewListItemDetails(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var car = e.SelectedItem as Car;

            await Navigation.PushAsync(new UserSelectedCarDetailPage(car));
        }

        void ViewListItemTappedDetails(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            Car car = (Car) e.Item;

            Console.WriteLine(car.FullName);
            Console.WriteLine(car.ModelYear);
            Console.WriteLine(car.Id);
            Console.WriteLine(car.Body);
            Console.WriteLine(e.ItemIndex);
        }

        async void DeleteCarFromListButton(object sender, System.EventArgs e)
        {
            Car car = (sender as MenuItem).BindingContext as Car;

            await App.CarManager.DeleteCar(car.Id);

            RefreshCars();
        }

        void PullToRefreshEvent(object sender, System.EventArgs e)
        {
            RefreshCars();
        }
    }
}
