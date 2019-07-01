using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class UpdateCarPage : ContentPage
    {
        private readonly int _carId;
        private Car _car;

        public UpdateCarPage(int id)
        {
            _carId = id;
            InitializeComponent();

            Task.WhenAll(CreateForm(true));
        }

        private async Task CreateForm(bool force = false)
        {
            _car = await App.CarManager.GetCar(_carId, force);
            var carDetails = await App.CarDetailManager.GetCarsDetail(_carId, force);


            var entryList = new List<EntryCell>
            {
                new EntryCell { Label = "Make", Text = _car.Make, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Manufacturer", Text = _car.Manufacturer, Placeholder = "Eg. Ford Werke AG" },
                new EntryCell { Label = "Plant", Text = _car.Plant, Placeholder = "Eg. Koeln-Niehl" },
                new EntryCell { Label = "Model Year", Text = _car.ModelYear, Placeholder = "Eg. 2010" },
                new EntryCell { Label = "Sequential Number", Text = _car.SequentialNumber, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Model", Text = _car.Model, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Body", Text = _car.Body, Placeholder = "Eg. SUV" },
                new EntryCell { Label = "Drive", Text = _car.Drive, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Number of Seats", Text = _car.NumberofSeats, Placeholder = "Eg. 5" },
                new EntryCell { Label = "Number of Doors", Text = _car.NumberofDoors, Placeholder = "Eg. 5" },
                new EntryCell { Label = "Steering", Text = _car.Steering, Placeholder = "Eg. LHD" },
                new EntryCell { Label = "Engine Displacement", Text = _car.EngineDisplacement, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Engine Cylinders", Text = _car.EngineCylinders, Placeholder = "Eg. 4" },
                new EntryCell { Label = "Number of Gears", Text = _car.NumberofGears, Placeholder = "Eg. 5" },
                new EntryCell { Label = "Engine", Text = _car.Engine, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Date of Manufacture", Text = _car.Made, Placeholder = "Eg. " },
                new EntryCell { Label = "Color", Text = _car.Color, Placeholder = "Eg. Blue" },
                new EntryCell { Label = "Fuel", Text = _car.Fuel, Placeholder = "Eg. Hybrid" },
                new EntryCell { Label = "Cubic Centimeters", Text = _car.Cc, Placeholder = "Eg. 1599" },
                new EntryCell { Label = "Power", Text = _car.Power, Placeholder = "Eg. 120" },
                new EntryCell { Label = "Emissions", Text = _car.Emissions, Placeholder = "Eg. Euro 4" },
                new EntryCell { Label = "Odometer", Text = _car.Odometer, Placeholder = "Eg. 100000" },
                new EntryCell { Label = "VIN", Text = _car.Vin, Placeholder = "17 characters" },
                new EntryCell { Label = "License Plate", Text = _car.License, Placeholder = "Eg. XX00XXX or X000XXX" }
            };

            foreach (var item in entryList)
            {
                if (item.Text != null)
                    Weknow.Add(item);
                else
                    Wedont.Add(item);
            }
        }

        private async void SaveDataButton(object sender, System.EventArgs e)
        {
            var updatedCar = new Car();

            var allEntries = new List<Cell>();

            foreach (var item in Weknow)
            {
                allEntries.Add(item);
            }

            foreach (var item in Wedont)
            {
                allEntries.Add(item);
            }


            foreach (EntryCell cell in allEntries)
            {
                switch (cell.Label)
                {
                    case "Make":
                        updatedCar.Make = cell.Text;
                        break;
                    case "Model Year":
                        updatedCar.ModelYear = cell.Text;
                        break;
                    case "Model":
                        updatedCar.Model = cell.Text;
                        break;
                    case "Body":
                        updatedCar.Body = cell.Text;
                        break;
                    case "Fuel":
                        updatedCar.Fuel = cell.Text;
                        break;
                    case "Odometer":
                        updatedCar.Odometer = cell.Text;
                        break;
                    case "VIN":
                        updatedCar.Vin = cell.Text;
                        break;
                    case "Manufacturer":
                        updatedCar.Manufacturer = cell.Text;
                        break;
                    case "Plant":
                        updatedCar.Plant = cell.Text;
                        break;
                    case "Sequential Number":
                        updatedCar.SequentialNumber = cell.Text;
                        break;
                    case "Drive":
                        updatedCar.Drive = cell.Text;
                        break;
                    case "Number of Seats":
                        updatedCar.NumberofSeats = cell.Text;
                        break;
                    case "Number of Doors":
                        updatedCar.NumberofDoors = cell.Text;
                        break;
                    case "Steering":
                        updatedCar.Steering = cell.Text;
                        break;
                    case "Engine Displacement":
                        updatedCar.EngineDisplacement = cell.Text;
                        break;
                    case "Engine Cylinders":
                        updatedCar.EngineCylinders = cell.Text;
                        break;
                    case "Number of Gears":
                        updatedCar.NumberofGears = cell.Text;
                        break;
                    case "Engine":
                        updatedCar.Engine = cell.Text;
                        break;
                    case "Date of Manufacture":
                        updatedCar.Made = cell.Text;
                        break;
                    case "Color":
                        updatedCar.Color = cell.Text;
                        break;
                    case "Cubic Centimeters":
                        updatedCar.Cc = cell.Text;
                        break;
                    case "Power":
                        updatedCar.Power = cell.Text;
                        break;
                    case "Emissions":
                        updatedCar.Emissions = cell.Text;
                        break;
                    case "License Plate":
                        updatedCar.License = cell.Text;
                        break;
                    default:
                        break;
                }
            }

            updatedCar.CarImage = _car.CarImage;

            var success = await App.CarManager.UpdateCar(_carId, updatedCar);
            if (success)
                UserDialogs.Instance.Toast(" Car successfully updated");
            else
                UserDialogs.Instance.Toast(" Error updating");
            await Navigation.PopAsync();
        }
    }
}
