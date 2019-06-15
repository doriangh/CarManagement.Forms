using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class BuyCarPage : ContentPage
    {
        private List<CarsSold> AllCarsSold;
        private List<Car> ListItems;

        public BuyCarPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            using(UserDialogs.Instance.Loading("Loading store data..."))
                await PopulateList();
        }

        private async Task PopulateList()
        {
            AllCarsSold = await App.CarsSoldManager.GetAll();
            ListItems = new List<Car>();

            foreach (var carSold in AllCarsSold)
            {
                var car = await App.CarManager.GetCar(carSold.CarId);

                ListItems.Add(car);
            }

            list.ItemsSource = ListItems;
        }

        async void Handle_Refreshing(object sender, System.EventArgs e)
        {
            await PopulateList();
            list.IsRefreshing = false;
        }

        async void User_Add_Car_Button(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new UserChooseCarToSellPage());
        }

        async void Filter_List_Button(object sender, System.EventArgs e)
        {
            var action = await DisplayActionSheet("How would you like to add the image?", "Cancel", null, "A -> Z",
                "Z -> A", "By Power", "By Odometer");

            switch (action)
            {
                case "A -> Z":
                    list.ItemsSource = ListItems.OrderBy(c => c.FullName);
                    break;
                case "Z -> A":
                    list.ItemsSource = ListItems.OrderByDescending(c => c.FullName);
                    break;
                case "By Power":
                    list.ItemsSource = ListItems.OrderBy(c => c.Power);
                    break;
                case "By Odometer":
                    list.ItemsSource = ListItems.OrderBy(c => c.Odometer);
                    break;
            }
        }

        void Clear_Button(object sender, System.EventArgs e)
        {
            list.ItemsSource = ListItems;
            search.Text = null;
        }

        void User_Search_Car_Button(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                list.ItemsSource = ListItems;
            }
            else
                list.ItemsSource = ListItems.Where(x => x.FullName.StartsWith(e.NewTextValue));
        }
    }
}