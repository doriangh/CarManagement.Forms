using System;
using System.Reflection;
using System.Xml;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta
{
    public partial class UserSelectedCarDetailPage : ContentPage
    {
        private string fuelKey = "daefd14b-9f2b-4968-9e4d-9d4bb4af01d1";

        private Car usercar;


        public UserSelectedCarDetailPage(Car car)
        {
            InitializeComponent();
            usercar = car;

            //var user = new PexelsClient("563492ad6f9170000100000100b4df8ef66f4f218c4eb57e60ac194d");

            //var results = user.SearchAsync(usercar.FullName + usercar.ModelYear).Result;

            //CarDetailLayout.Children.Add(new Image { Source = ImageSource.FromUri(new Uri(results.Photos[0].Src.Original)) });

            String url = "http://www.carimagery.com/api.asmx/GetImageUrl?searchTerm=" + usercar.Make + "+" + usercar.Model + ("+" + usercar.ModelYear) ?? null + ("+" + usercar.Body) ?? null;

            XmlTextReader reader = new XmlTextReader(url);

            while (reader.Read())
            {
                if (reader.Value.Contains("http://"))
                {
                    CarDetailLayout.Children.Add(new Image { Source = ImageSource.FromUri(new Uri(reader.Value.Trim())) });
                }
                //Console.WriteLine(reader.Value.Trim());

            }

            addVin.IsVisible |= usercar.VIN != null;

            PropertyInfo[] properties = usercar.GetType().GetProperties();
            foreach (var item in properties)
            {
                if (item.GetValue(usercar, null) is null)
                {
                    continue;
                }
                CarDetailLayout.Children.Add(new Label { Text = item.Name + ": " + item.GetValue(usercar, null) });
            }
        }

        void EditCarButton(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        void AddVINButton(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        async void DeleteCarButton(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
