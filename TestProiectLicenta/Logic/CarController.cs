using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SQLite;
using TestProiectLicenta.Models;
using TestProiectLicenta.Persistence;
using Xamarin.Forms;

namespace TestProiectLicenta.Logic
{
    public class CarController
    {
        private SQLiteConnection _connection;
        MySQLOperations operations = new MySQLOperations();

        public CarController()
        {
            _connection = DependencyService.Get<ISQLiteDB>().GetConnection();
            _connection.CreateTable<Car>();
        }

        public void AddCar(JObject data, int userId, string VIN)
        {

            Car car = new Car();

            foreach (var item in data["decode"])
            {
                car.Make = car.Make ?? (item["label"].ToString() == "Make" ? item["value"].ToString() : null);
                car.Manufacturer = car.Manufacturer ?? (item["label"].ToString() == "Manufacturer" ? item["value"].ToString() : null);
                car.Plant = car.Plant ?? (item["label"].ToString() == "Manufacturer Address" ? item["value"].ToString() : null);
                car.ModelYear = car.ModelYear ?? (item["label"].ToString() == "Model Year" ? item["value"].ToString() : null);
                car.Model = car.Model ?? (item["label"].ToString() == "Model" ? item["value"].ToString() : null);
                car.Body = car.Body ?? (item["label"].ToString() == "Body" ? item["value"].ToString() : null);
                car.Drive = car.Drive ?? (item["label"].ToString() == "Drive" ? item["value"].ToString() : null);
                car.NumberofSeats = car.NumberofSeats ?? (item["label"].ToString() == "Number of Seats" ? item["value"].ToString() : null);
                car.NumberofDoors = car.NumberofDoors ?? (item["label"].ToString()== "Number of Doors" ? item["value"].ToString() : null);
                car.Steering = car.Steering ?? (item["label"].ToString() == "Steering" ? item["value"].ToString() : null);
                car.CC = car.CC ?? (item["label"].ToString() == "Engine Displacement (ccm)" ? item["value"].ToString() : null);
                car.EngineCylinders = car.EngineCylinders ?? (item["label"].ToString() == "Engine Cylinders" ? item["value"].ToString() : null);
                car.Transmission = car.Transmission ?? (item["label"].ToString() == "Transmission" ? item["value"].ToString() : null);
                car.NumberofGears = car.NumberofGears ?? (item["label"].ToString() == "Number of Gears" ? item["value"].ToString() : null);
                car.Color = car.Color ?? (item["label"].ToString() == "Color" ? item["value"].ToString() : null);
                car.Engine = car.Engine ?? (item["label"].ToString() == "Engine (full)" ? item["value"].ToString() : null);
                car.Fuel = car.Fuel ?? (item["label"].ToString() == "Fuel Type - Primary" ? item["value"].ToString() : null);
                car.Power = car.Power ?? (item["label"].ToString() == "Engine Power (kW)" ? item["value"].ToString() : null);
                car.Made = car.Made ?? (item["label"].ToString() == "Made" ? item["value"].ToString() : null);
                car.Emissions = car.Emissions ?? (item["label"].ToString() == "Emission Standard" ? item["value"].ToString() : null);
                //Odometer = data["decode"][2]["value"].ToString()
            }
            car.UserId = userId;
            car.VIN = VIN;

            _connection.Insert(car);
            operations.AddCar(car);

        }

        public List<Car> GetUserCars(int userId)
        {
            return _connection.Table<Car>().Where(c => c.UserId == userId).ToList();
        }

        public void DeleteCar (Car car)
        {
            _connection.Delete(car);
            operations.DeleteCar(car);
        }

        public void AddVin (Car car, string newVIN)
        {
            car.VIN = newVIN;
            _connection.Update(car);
            operations.UpdateCar(car);
        }
    }
}
