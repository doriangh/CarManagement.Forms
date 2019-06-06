using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using TestProiectLicenta.Models;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class UserPageForm : TabbedPage
    {
        private ObservableCollection<ListCarAttributes> _listData;

        private ObservableCollection<Car> _userCars;
        private string _userId;

        public UserPageForm()
        {
            InitializeComponent();

            Fid.IsToggled = Convert.ToBoolean(Application.Current.Properties["FaceID"]);

            InitCars();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async Task InitCars()
        {
            using (UserDialogs.Instance.Loading("Getting user details.\nHold on"))
            {
                _userId = await SecureStorage.GetAsync("UserId");

                var user = await App.UserManager.GetUserById(Convert.ToInt32(_userId));

                BindingContext = user;

                navName.Text = user.Name;

                name.Text = user.Name;

                if (user.UserImage != null && user.UserImage.Contains("https://"))
                {
                    topAvatar.Source = ImageSource.FromUri(new Uri(user.UserImage));

                    Avatar.Source = ImageSource.FromUri(new Uri(user.UserImage));
                }

                //userCars = new ObservableCollection<Car>();

                var lst = await App.CarManager.GetUserCars(user.Id);
                var lstDetail = await App.CarDetailManager.GetCarDetails();

                _listData = new ObservableCollection<ListCarAttributes>();

                foreach (var car in lst)
                {
                    var carDetail = lstDetail.Find(x => x.CarId == car.Id);
                    var listItem = ConvertToListAttribute(car, carDetail);

                    if (listItem != null) _listData.Add(listItem);
                }

                if (_listData.Count > 0)
                {
                    Cars.IsVisible = true;
                    AddCar.IsVisible = false;
                    Cars.ItemsSource = _listData;
                    Cars.RowHeight = 200;
                    Cars.HasUnevenRows = true;
                }
            }
        }

        private ListCarAttributes ConvertToListAttribute(Car car, CarDetail carDetail)
        {
            //var carDetail = await App.CarDetailManager.GetCarsDetail(car.Id);
            //var carDetail = carDetails.Find(x => x.CarId == car.Id);

            if (carDetail == null) return null;
            var nextITP = carDetail.Itp.AddYears(2);


            var carAttributes = new ListCarAttributes
            {
                CarId = car.Id,
                FullName = car.FullName,
                ModelYear = car.ModelYear,
                RemainingItp = (carDetail.Itp.AddYears(2) - DateTime.Today).TotalDays.ToString(),
                RemainingRoadTax = (carDetail.RoadTax - DateTime.Today).TotalDays.ToString(),
                RemainingOilChange = (carDetail.OilChange - DateTime.Today).TotalDays.ToString(),
                RoadTaxValue = carDetail.RoadTaxValue.ToString(),
                InsuranceValue = carDetail.InsuranceValue.ToString(),
                CarImage = car.CarImage,
                TaxValue = carDetail.TaxValue == -1 ? "Nu putem calcula" : carDetail.TaxValue.ToString()
            };


            return carAttributes;

        }

        private async Task RefreshCars()
        {
            var query =
                from car in await App.CarManager.GetCarsAsync()
                join detail in await App.CarDetailManager.GetCarDetails()
                    on car.Id equals detail.CarId
                where car.UserId == Convert.ToInt32(_userId)
                select car;

            var carDetails = await App.CarDetailManager.GetCarDetails();

            var dataQuery = query.ToList();

            if (_listData.Count < dataQuery.Count)
                foreach (var car in dataQuery)
                {
                    var carDetail = carDetails.Find(x => x.CarId == car.Id);

                    var listCar = ConvertToListAttribute(car, carDetail);

                    if (_listData.Contains(listCar)) continue;
                    _listData.Add(listCar);
                }
            else if (_listData.Count > dataQuery.Count)
                foreach (var listItem in _listData)
                foreach (var car in dataQuery)
                {
                    if (listItem.CarId == car.Id) continue;

                    _listData.Remove(listItem);
                }
        }

        private async void AddCarsWhenNoCarsButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCarPage());
        }

        private async void BackButtonPressed(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void AddVinButtonPressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCarVinPage());
        }

        private async void SignOutButton(object sender, EventArgs e)
        {
            SecureStorage.RemoveAll();

            await Navigation.PopToRootAsync();
        }

        private async void ViewListItemDetails(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;


            var listItem = e.SelectedItem as ListCarAttributes;

            var car = await App.CarManager.GetCar(listItem.CarId);

            await Navigation.PushAsync(new UserSelectedCarDetailPage(car));

            ((ListView) sender).SelectedItem = null;
        }

        private void ViewListItemTappedDetails(object sender, ItemTappedEventArgs e)
        {
            var car = (Car) e.Item;

            Console.WriteLine(car.FullName);
            Console.WriteLine(car.ModelYear);
            Console.WriteLine(car.Id);
            Console.WriteLine(car.Body);
            Console.WriteLine(e.ItemIndex);
        }

        private async void DeleteCarFromListButton(object sender, EventArgs e)
        {
            var listItem = ((MenuItem) sender).BindingContext as ListCarAttributes;

            var carDetail = await App.CarDetailManager.GetCarsDetail(listItem.CarId);

            _listData.Remove(listItem);

            await App.CarManager.DeleteCar(listItem.CarId);
            await App.CarDetailManager.DeleteCarDetail(carDetail.Id);
        }

        private async void Handle_Refreshing(object sender, EventArgs e)
        {
            Cars.IsRefreshing = true;

            await RefreshCars();

            Cars.IsRefreshing = false;
        }

        private void Handle_Toggled(object sender, ToggledEventArgs e)
        {
            Application.Current.Properties["FaceID"] = Fid.IsToggled;
        }

        void Search_Bar_Text(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
                Cars.ItemsSource = _listData;
            else
                Cars.ItemsSource = _listData.Where(x => x.FullName.StartsWith(e.NewTextValue));
        }

        void Search_Car_Button(object sender, System.EventArgs e)
        {
            var searchbar = new SearchBar
            {
                Placeholder = "eg. Ford Focus",
                Opacity = 0
            };

            if (Cars.Header == null)
            {
                searchbar.TextChanged += Search_Bar_Text;
                Cars.Header = searchbar;
                searchbar.FadeTo(1);
            }
            else
            {
                searchbar.FadeTo(0);
                Cars.Header = null;
                Cars.ItemsSource = _listData;
            }

        }
    }
}