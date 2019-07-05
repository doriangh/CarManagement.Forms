using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Connectivity;
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
                TaxValue = carDetail.TaxValue == -1 ? "Could not calculate" : carDetail.TaxValue.ToString(),
                Price = carDetail.Price,
                WinterTyres = carDetail.WinterTires
            };

            if ((carDetail.Itp.AddYears(2) - DateTime.Today).Days > 0){
                carAttributes.ITPColor = "#8BC34A";
                carAttributes.ITPIcon = "good";
                carAttributes.RefreshITP = false;
            }
            else
            {
                carAttributes.ITPColor = "#F44336";
                carAttributes.ITPIcon = "bad";
                carAttributes.RefreshITP = true;
            }

            if ((carDetail.RoadTax.AddDays(carDetail.RoadTaxPeriod) - DateTime.Today).Days > 0)
            {
                carAttributes.RoadTaxColor = "#8BC34A";
                carAttributes.RoadTaxIcon = "good";
                carAttributes.RefreshRoadTax = false;
            }
            else
            {
                carAttributes.RoadTaxColor = "#F44336";
                carAttributes.RoadTaxIcon = "bad";
                carAttributes.RefreshRoadTax = true;
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
                carAttributes.RefreshInsurance = false;
            }
            else
            {
                carAttributes.InsuranceColor = "#F44336";
                carAttributes.InsuranceIcon = "bad";
                carAttributes.RefreshInsurance = true;
            }

            if (!carAttributes.WinterTyres && DateTime.Today.Month > 4 && DateTime.Today.Month < 10)
            {
                carAttributes.TiresIcon = "good";
                carAttributes.TiresColor = "#8BC34A";
                carAttributes.TiresText = "You don't need to change your tires";
                carAttributes.WinterTyres = false;
            }
            else if (carAttributes.WinterTyres && DateTime.Today.Month > 10 && DateTime.Today.Month < 4)
            {
                carAttributes.TiresIcon = "good";
                carAttributes.TiresColor = "#8BC34A";
                carAttributes.TiresText = "You don't need tochange your tires!";
                carAttributes.WinterTyres = false;
            }
            else
            {
                carAttributes.TiresIcon = "bad";
                carAttributes.TiresColor = "#F44336";
                carAttributes.TiresText = "You should change your tires!";
                carAttributes.RefreshTires = true;
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
            {
                carousel.IsVisible = true;
                carousel.ItemsSource = _listData.OrderByDescending(c => c.CarId);
                search.IsEnabled = true;
                details.IsEnabled = true;
                nocars.IsVisible = false;
            }
            else
            {
                nocars.IsVisible = true;
                search.IsEnabled = false;
                details.IsEnabled = false;
                carousel.IsVisible = false;
            }
        }

        private async void AddCarsWhenNoCarsButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChooseMethodFormPage());
        }

        private async void ViewListItemDetails(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;

            var listItem = e.SelectedItem as ListCarAttributes;

            var car = await App.CarManager.GetCar(listItem.CarId, true);
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
                searchBar.IsVisible = false;
                searchBar.FadeTo(1);
                
                carousel.ItemsSource = _listData;
            }
        }

        private async void MoreInfoClicked(object sender, System.EventArgs e)
        {
            using (UserDialogs.Instance.Loading("Please wait..."))
            {
                var item = (ListCarAttributes)carousel[carousel.SelectedIndex];
                Car car;

                if (CrossConnectivity.Current.IsConnected)
                    car = await App.CarManager.GetCar(item.CarId, true);
                else
                    car = await App.CarManager.GetCar(item.CarId);
                
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

        async void Update_Insurance_Date(object sender, System.EventArgs e)
        {
            if (await DisplayAlert("Are you sure?", "Are you sure you want to update your insurance period?", "Yes", "No"))
            {

                var result = await DisplayActionSheet("Choose new insurance period", "Cancel", null, "6 Months", "A Year");

                var car = (ListCarAttributes)carousel[carousel.SelectedIndex];
                var updateCar = await App.CarDetailManager.GetCarsDetail(car.CarId);

                switch (result)
                {
                    case "6 Months":
                        updateCar.InsurancePeriod = 6;
                        break;
                    case "A Year":
                        updateCar.InsurancePeriod = 12;
                        break;
                    default:
                        return;
                }

                var day = await UserDialogs.Instance.DatePromptAsync("When did you pay your insurance?", DateTime.Today);
                if (day.Ok)
                {
                    updateCar.Insurance = day.SelectedDate;
                    await Task.WhenAll(App.CarDetailManager.UpdateCarDetail(updateCar), RefreshCars(true));
                }

            }
        }

        async void Update_ITP_Date(object sender, System.EventArgs e)
        {
            var car = (ListCarAttributes)carousel[carousel.SelectedIndex];
            var updateCar = await App.CarDetailManager.GetCarsDetail(car.CarId);

            var response = await DisplayAlert("Are you sure?", "Are you sure you want to update your ITP?", "Yes", "No");

            if (response)
            {
                var date = await UserDialogs.Instance.DatePromptAsync("Choose ITP Date", DateTime.Today);

                if (date.Ok)
                {
                    updateCar.Itp = DateTime.Today;
                    await Task.WhenAll(App.CarDetailManager.UpdateCarDetail(updateCar), RefreshCars(true));
                }
            }
        }

        async void Update_Road_Tax(object sender, System.EventArgs e)
        {
            if (await DisplayAlert("Are you sure?", "Are you sure you want to update your road tax period?", "Yes", "No"))
            {
                var result = await DisplayActionSheet("Choose your new road tax period", "Cancel", null, "7 Days", "30 Days", "90 Days", "A Year");

                var car = (ListCarAttributes)carousel[carousel.SelectedIndex];
                var updateCar = await App.CarDetailManager.GetCarsDetail(car.CarId);


                switch (result)
                {
                    case "7 Days":
                        updateCar.RoadTaxPeriod = 7;    
                        break;
                    case "30 Days":
                        updateCar.RoadTaxPeriod = 30;
                        break;
                    case "90 Days":
                        updateCar.RoadTaxPeriod = 90;
                        break;
                    case "A Year":
                        updateCar.RoadTaxPeriod = 365;
                        break;
                    default:
                        return;
                }

                var day = await UserDialogs.Instance.DatePromptAsync("When did you pay your road tax?", DateTime.Today);
                if (day.Ok)
                {
                    updateCar.RoadTax = day.SelectedDate;
                    await Task.WhenAll(App.CarDetailManager.UpdateCarDetail(updateCar), RefreshCars(true));
                }
            }
        }

        async void Change_Tires(object sender, System.EventArgs e)
        {
            if (await DisplayAlert("Are you sure?", "Are you sure you have changed your tires?", "Yes", "No"))
            {
                var car = (ListCarAttributes)carousel[carousel.SelectedIndex];
                var updateCar = await App.CarDetailManager.GetCarsDetail(car.CarId);

                updateCar.WinterTires = !updateCar.WinterTires;
                await Task.WhenAll(App.CarDetailManager.UpdateCarDetail(updateCar), RefreshCars(true));
            }
        }

        async void Update_Oil(object sender, System.EventArgs e)
        {

            var car = (ListCarAttributes)carousel[carousel.SelectedIndex];
            var userCar = await App.CarManager.GetCar(car.CarId);
            var updateCar = await App.CarDetailManager.GetCarsDetail(car.CarId);

            if (updateCar.OilChange > 0)
            {

                var newOdometer = await UserDialogs.Instance.PromptAsync(new PromptConfig
                {
                    Title = "Please enter the current odometer",
                    Placeholder = string.Format("Currently: {0}KM", userCar.Odometer),
                    OkText = "OK",
                    CancelText = "Cancel",
                    MaxLength = 7,
                    InputType = InputType.Number,
                });

                if (newOdometer.Ok)
                {
                    userCar.Odometer = newOdometer.Text;

                    updateCar.OilChange = Convert.ToInt32(newOdometer.Text) % 15000;

                    await Task.WhenAll(App.CarManager.UpdateCar(userCar.Id, userCar), App.CarDetailManager.UpdateCarDetail(updateCar), RefreshCars(true));
                }
                else
                {
                    await DisplayAlert("Could not update oil change", "We need the new odometer value in order to show how many KM until the next oil change.", "OK");
                }
            }
            else
            {
                var result = await DisplayAlert("Change now!", "Did you change your oil?", "Yes", "No");
                if (result)
                {
                    var newOdometer = await UserDialogs.Instance.PromptAsync(new PromptConfig
                    {
                        Title = "Please enter the current odometer",
                        Placeholder = string.Format("Currently: {0}KM", userCar.Odometer),
                        OkText = "OK",
                        CancelText = "Cancel",
                        MaxLength = 7,
                        InputType = InputType.Number,
                    });

                    if (newOdometer.Ok)
                    {
                        userCar.Odometer = newOdometer.Text;

                        updateCar.OilChange = Convert.ToInt32(newOdometer.Text) % 15000;

                        await Task.WhenAll(App.CarManager.UpdateCar(userCar.Id, userCar), App.CarDetailManager.UpdateCarDetail(updateCar), RefreshCars(true));
                    }
                    else
                    {
                        await DisplayAlert("Could not update oil change", "We need the new odometer value in order to show how many KM until the next oil change.", "OK");
                    }
                }
            }
        }
    }
}
