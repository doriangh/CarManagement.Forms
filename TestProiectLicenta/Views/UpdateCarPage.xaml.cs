using System.Collections.Generic;
using Acr.UserDialogs;
using TestProiectLicenta.Models;
using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class UpdateCarPage : ContentPage
    {
        private readonly int _carId;
        private Car car;

        public UpdateCarPage(int id)
        {
            _carId = id;

            CreateForm();

            InitializeComponent();
        }

        private async void CreateForm()
        {
            car = await App.CarManager.GetCar(_carId);
            var carDetails = await App.CarDetailManager.GetCarsDetail(_carId);


            var entryList = new List<EntryCell>
            {
                new EntryCell { Label = "Make", Text = car.Make, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Manufacturer", Text = car.Manufacturer, Placeholder = "Eg. Ford Werke AG" },
                new EntryCell { Label = "Plant", Text = car.Plant, Placeholder = "Eg. Koeln-Niehl" },
                new EntryCell { Label = "Model Year", Text = car.ModelYear, Placeholder = "Eg. 2010" },
                new EntryCell { Label = "Sequential Number", Text = car.SequentialNumber, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Model", Text = car.Model, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Body", Text = car.Body, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Drive", Text = car.Drive, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Number of Seats", Text = car.NumberofSeats, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Number of Doors", Text = car.NumberofDoors, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Steering", Text = car.Steering, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Engine Displacement", Text = car.EngineDisplacement, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Engine Cylinders", Text = car.EngineCylinders, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Number of Gears", Text = car.NumberofGears, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Engine", Text = car.Engine, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Date of Manufacture", Text = car.Made, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Color", Text = car.Color, Placeholder = "Eg. Blue" },
                new EntryCell { Label = "Fuel", Text = car.Fuel, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Cubic Centimeters", Text = car.Cc, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Power", Text = car.Power, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Emissions", Text = car.Emissions, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "Odometer", Text = car.Odometer, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "VIN", Text = car.Vin, Placeholder = "Eg. Ford" },
                new EntryCell { Label = "License Plate", Text = car.License, Placeholder = "Eg. Ford" }
            };

            foreach (var item in entryList)
            {
                if (item.Text != null)
                    Weknow.Add(item);
                else
                    Wedont.Add(item);
            }
        }

        async void SaveDataButton(object sender, System.EventArgs e)
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

            updatedCar.CarImage = car.CarImage;

            var success = await App.CarManager.UpdateCar(_carId, updatedCar);
            if (success)
                UserDialogs.Instance.Toast(" Car successfully updated");
            else
                UserDialogs.Instance.Toast(" Error updating");
            await Navigation.PopAsync();
        }
    }
}
