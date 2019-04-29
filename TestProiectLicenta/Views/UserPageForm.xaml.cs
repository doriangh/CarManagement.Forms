using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Acr.UserDialogs;
using TestProiectLicenta.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class UserPageForm : TabbedPage
    {
        public string userId;

        ObservableCollection<Car> userCars;

        public UserPageForm()
        {
            InitializeComponent();

            fid.IsToggled = Convert.ToBoolean(Application.Current.Properties["FaceID"]);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            using (UserDialogs.Instance.Loading("Getting user details.\nHold on"))
            {

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
            }
        }

        async Task RefreshCars()
        {
            var lst = await App.CarManager.GetUserCars(Convert.ToInt32(userId));

            if (userCars.Count < lst.Count)
            {
                foreach (var car in lst)
                {
                    if (userCars.Contains(car)) continue;
                    else userCars.Add(car);
                }
            }
            else if (userCars.Count > lst.Count)
            {
                foreach (var car in lst)
                {
                    if (userCars.Contains(car)) continue;
                    else userCars.Remove(car);
                }
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

            userCars.Remove(car);

            await App.CarManager.DeleteCar(car.Id);
        }

        async void Handle_Refreshing(object sender, System.EventArgs e)
        {
            cars.IsRefreshing = true;

            await RefreshCars();

            cars.IsRefreshing = false;
        }

        void Handle_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Application.Current.Properties["FaceID"] = fid.IsToggled;
        }
    }
}
