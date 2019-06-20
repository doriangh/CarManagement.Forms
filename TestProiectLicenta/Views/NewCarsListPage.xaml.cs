using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using TestProiectLicenta.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class NewCarsListPage : ContentPage
    {
        private string _userId;
        private ObservableCollection<ListCarAttributes> _listData;

        public NewCarsListPage()
        {
            InitializeComponent();
            Task.WhenAll(PopulateView());
        }

        private async Task PopulateView()
        {
            loading.IsVisible = true;
            loading.IsRunning = true;

            _userId = await SecureStorage.GetAsync("UserId");

            await Task.WhenAll(RefreshCars());

            loading.IsVisible = false;
            loading.IsRunning = false;
        }

        private int GetRoadTaxPeriod(CarDetail carDetail)
        {
            switch (carDetail.RoadTaxPeriod)
            {
                case 0:
                    return 7;
                case 1:
                    return 30;
                case 2:
                    return 90;
                case 3:
                    return 365;
                default:
                    return 0;
            }
        }

        private int CalculateITPPeriod(Car car)
        {
            if (Convert.ToInt32(car.ModelYear) == DateTime.Today.Year){
                return 3;
            }
            else if (Convert.ToInt32(car.ModelYear) > DateTime.Today.AddYears(-12).Year)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        private async Task<ListCarAttributes> ConvertToListAttribute(Car car, CarDetail carDetail)
        {
            if (carDetail == null) return null;

            var carImage = await App.CarImagesManager.GetCarsImages(car.Id);

            var carAttributes = new ListCarAttributes
            {
                CarId = car.Id,
                FullName = car.FullName,
                ModelYear = car.ModelYear,
                RemainingItp = (carDetail.Itp.AddYears(CalculateITPPeriod(car)) - DateTime.Today).Days.ToString(),
                RemainingRoadTax = (carDetail.RoadTax.AddDays(carDetail.RoadTaxPeriod) - DateTime.Today).Days.ToString(),
                RemainingOilChange = carDetail.OilChange.ToString(),
                RoadTaxValue = carDetail.RoadTaxValue.ToString(),
                RemainingInsurance = (carDetail.Insurance.AddMonths(carDetail.InsurancePeriod) - DateTime.Today).Days.ToString(),
                CarImage = car.CarImage ?? (carImage?[0].CarImage),
                TaxValue = carDetail.TaxValue == -1 ? "Nu putem calcula" : carDetail.TaxValue.ToString(),
                Price = carDetail.Price
            };

            if ((carDetail.Itp.AddYears(2) - DateTime.Today).Days > 0){
                carAttributes.ITPColor = "#8BC34A";
                carAttributes.ITPIcon = "good";
            }
            else
            {
                carAttributes.ITPColor = "#F44336";
                carAttributes.ITPIcon = "bad";
            }

            if ((carDetail.RoadTax.AddDays(carDetail.RoadTaxPeriod) - DateTime.Today).Days > 0)
            {
                carAttributes.RoadTaxColor = "#8BC34A";
                carAttributes.RoadTaxIcon = "good";
            }
            else
            {
                carAttributes.RoadTaxColor = "#F44336";
                carAttributes.RoadTaxIcon = "bad";
            }

            if (carDetail.OilChange > 0)
            {
                carAttributes.OilColor = "#8BC34A";
                carAttributes.OilIcon = "good";
            }
            else
            {
                carAttributes.OilColor = "#F44336";
                carAttributes.OilIcon = "bad";
            }
            if (Convert.ToInt32(carAttributes.RemainingInsurance) > 0)
            {
                carAttributes.InsuranceColor = "#8BC34A";
                carAttributes.InsuranceIcon = "good";
            }
            else
            {
                carAttributes.InsuranceColor = "#F44336";
                carAttributes.InsuranceIcon = "bad";
            }


            return carAttributes;

        }

        private async Task RefreshCars(bool force = false)
        {
            var lst = await App.CarManager.GetUserCars(Convert.ToInt32(_userId), force);
            var lstDetail = await App.CarDetailManager.GetCarDetails(force);

            _listData = new ObservableCollection<ListCarAttributes>();

            foreach (var car in lst)
            {
                var carDetail = lstDetail.Find(x => x.CarId == car.Id);
                var listItem = await ConvertToListAttribute(car, carDetail);

                if (listItem != null) _listData.Add(listItem);
            }

            if (_listData.Count > 0)
                carousel.ItemsSource = _listData.OrderByDescending(c => c.CarId);
            else
                nocars.IsVisible = true;
        }

        private async void AddCarsWhenNoCarsButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChooseMethodFormPage());
        }

        private async void ViewListItemDetails(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            var listItem = e.SelectedItem as ListCarAttributes;

            var car = await App.CarManager.GetCar(listItem.CarId);
            if (car != null)
            {
                await Navigation.PushAsync(new UserSelectedCarDetailPage(car));
                ((ListView)sender).SelectedItem = null;
            }
            else
            {
                ((ListView)sender).SelectedItem = null;
                await DisplayAlert("No internet connection", "It appears you have no internet connection.\nPlease try again.", "OK");

            }
        }

        private void ViewListItemTappedDetails(object sender, ItemTappedEventArgs e)
        {
            var car = (Car)e.Item;

            Console.WriteLine(car.FullName);
            Console.WriteLine(car.ModelYear);
            Console.WriteLine(car.Id);
            Console.WriteLine(car.Body);
            Console.WriteLine(e.ItemIndex);
        }

        private async void DeleteCarFromListButton(object sender, EventArgs e)
        {
            var listItem = ((MenuItem)sender).BindingContext as ListCarAttributes;

            var carDetail = await App.CarDetailManager.GetCarsDetail(listItem.CarId);

            _listData.Remove(listItem);

            await App.CarManager.DeleteCar(listItem.CarId);
            await App.CarDetailManager.DeleteCarDetail(carDetail.Id);
        }

        private async void Handle_Refreshing(object sender, EventArgs e)
        { 

            await RefreshCars(true);

        }

        private void Search_Bar_Text(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                carousel.ItemsSource = _listData;
                carousel.SelectedIndex = 0;
            }
            else
                carousel.ItemsSource = _listData.Where(x => x.FullName.StartsWith(e.NewTextValue));
        }

        private void Search_Car_Button(object sender, System.EventArgs e)
        {
            var searchbar = new SearchBar
            {
                Placeholder = "eg. Ford Focus",
                Opacity = 0
            };

            if (searchBar.IsVisible == false)
            {
                searchBar.IsVisible = true;
                searchBar = searchbar;
                searchbar.FadeTo(1);
            }
            else
            {
                searchBar = null;
                
                carousel.ItemsSource = _listData;
            }
        }

        private async void MoreInfoClicked(object sender, System.EventArgs e)
        {
            using (UserDialogs.Instance.Loading("Please wait..."))
            {
                var item = (ListCarAttributes)carousel[carousel.SelectedIndex];

                var car = await App.CarManager.GetCar(item.CarId);
                if (car != null)
                {
                    await Navigation.PushModalAsync(new NavigationPage(new UserSelectedCarDetailPage(car)));
                }
                else
                {
                    ((ListView)sender).SelectedItem = null;
                    await DisplayAlert("No internet connection", "It appears you have no internet connection.\nPlease try again.", "OK");

                }
            }
        }
    }
}
