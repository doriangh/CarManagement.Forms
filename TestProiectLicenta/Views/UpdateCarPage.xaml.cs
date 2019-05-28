using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TestProiectLicenta.Views
{
    public partial class UpdateCarPage : ContentPage
    {
        private readonly int _carId;

        public UpdateCarPage(int id)
        {
            _carId = id;

            CreateForm();
            InitializeComponent();
        }

        private async void CreateForm()
        {

            var car = await App.CarManager.GetCar(_carId);
            var carDetails = await App.CarDetailManager.GetCarsDetail(_carId);


            if (car.Make != null)
                Weknow.Add(new EntryCell { Label = "Make", Text = car.Make });
            else
                Wedont.Add(new EntryCell { Label = "Make", Placeholder = "Eg. Ford" });

            if (car.Manufacturer != null)
                Weknow.Add(new EntryCell { Label = "Manufacturer", Text = car.Manufacturer });
            else
                Wedont.Add(new EntryCell { Label = "Manufacturer", Placeholder = "Eg. Ford Werke AG" });

            if (car.Plant != null)
                Weknow.Add(new EntryCell { Label = "Plant", Text = car.Plant });
            else
                Wedont.Add(new EntryCell { Label = "Plant", Placeholder = "Eg. Koeln-Niehl" });

            if (car.ModelYear != null)
                Weknow.Add(new EntryCell { Label = "ModelYear", Text = car.ModelYear});
            else
                Wedont.Add(new EntryCell { Label = "ModelYear", Placeholder = "Eg. 2010" });

            if (car.SequentialNumber != null)
                Weknow.Add(new EntryCell { Label = "SequentialNumber", Text = car.SequentialNumber });
            else
                Wedont.Add(new EntryCell { Label = "SequentialNumber", Placeholder = "" });

            if (car.Model != null)
                Weknow.Add(new EntryCell { Label = "Model", Text = car.Model });
            else
                Wedont.Add(new EntryCell { Label = "Model", Placeholder = "Eg. Focus" });

            if (car.Body != null)
                Weknow.Add(new EntryCell { Label = "Body", Text = car.Body });
            else
                Wedont.Add(new EntryCell { Label = "Body", Placeholder = "Eg. Hatchback" });

            if (car.Drive != null)
                Weknow.Add(new EntryCell { Label = "Drive", Text = car.Drive });
            else
                Wedont.Add(new EntryCell { Label = "Drive", Placeholder = "Eg. Front wheel" });

            if (car.NumberofSeats != null)
                Weknow.Add(new EntryCell { Label = "NumberofSeats", Text = car.NumberofSeats });
            else
                Wedont.Add(new EntryCell { Label = "NumberofSeats", Placeholder = "Eg. 5" });

            if (car.NumberofDoors != null)
                Weknow.Add(new EntryCell { Label = "NumberofDoors", Text = car.NumberofDoors });
            else
                Wedont.Add(new EntryCell { Label = "NumberofDoors", Placeholder = "Eg. 5" });

            if (car.Steering != null)
                Weknow.Add(new EntryCell { Label = "Steering", Text = car.Steering });
            else
                Wedont.Add(new EntryCell { Label = "Steering", Placeholder = "Eg. Left hand side" });

            if (car.EngineDisplacement != null)
                Weknow.Add(new EntryCell { Label = "EngineDisplacement", Text = car.EngineDisplacement });
            else
                Wedont.Add(new EntryCell { Label = "EngineDisplacement", Placeholder = "" });

            if (car.EngineCylinders != null)
                Weknow.Add(new EntryCell { Label = "EngineCylinders", Text = car.EngineCylinders });
            else
                Wedont.Add(new EntryCell { Label = "EngineCylinders", Placeholder = "Eg. 4" });

            if (car.NumberofGears != null)
                Weknow.Add(new EntryCell { Label = "NumberofGears", Text = car.NumberofGears });
            else
                Wedont.Add(new EntryCell { Label = "NumberofGears", Placeholder = "Eg. 6" });

            if (car.Engine != null)
                Weknow.Add(new EntryCell { Label = "Engine", Text = car.Engine });
            else
                Wedont.Add(new EntryCell { Label = "Engine", Placeholder = "Eg. Ingenium" });

            if (car.Made != null)
                Weknow.Add(new EntryCell { Label = "Made", Text = car.Made });
            else
                Wedont.Add(new EntryCell { Label = "Made", Placeholder = "Date of manufacture" });

            if (car.Color != null)
                Weknow.Add(new EntryCell { Label = "Color", Text = car.Color });
            else
                Wedont.Add(new EntryCell { Label = "Color", Placeholder = "Eg. Blue" });

            if (car.Fuel != null)
                Weknow.Add(new EntryCell { Label = "Fuel", Text = car.Fuel });
            else
                Wedont.Add(new EntryCell { Label = "Fuel", Placeholder = "Eg. Diesel" });

            if (car.Cc != null)
                Weknow.Add(new EntryCell { Label = "CC", Text = car.Cc });
            else
                Wedont.Add(new EntryCell { Label = "CC", Placeholder = "Eg. 2000" });

            if (car.Power != null)
                Weknow.Add(new EntryCell { Label = "Power", Text = car.Power });
            else
                Wedont.Add(new EntryCell { Label = "Power", Placeholder = "Eg. 89" });

            if (car.Emissions != null)
                Weknow.Add(new EntryCell { Label = "Emissions", Text = car.Emissions });
            else
                Wedont.Add(new EntryCell { Label = "Emissions", Placeholder = "Eg. Euro 5" });

            if (car.Odometer != null)
                Weknow.Add(new EntryCell { Label = "Odometer", Text = car.Odometer });
            else
                Wedont.Add(new EntryCell { Label = "Odometer", Placeholder = "Eg. 123456" });

            if (car.Vin != null)
                Weknow.Add(new EntryCell { Label = "VIN", Text = car.Vin });
            else
                Wedont.Add(new EntryCell { Label = "VIN", Placeholder = "17 Characters" });

            if (car.License != null)
                Weknow.Add(new EntryCell { Label = "License", Text = car.License });
            else
                Wedont.Add(new EntryCell { Label = "License", Placeholder = "Eg. B123ABC" });
        }
    }
}
