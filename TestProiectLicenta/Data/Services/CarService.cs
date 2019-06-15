using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MonkeyCache.FileStore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using TestProiectLicenta.Data.Interfaces;
using TestProiectLicenta.Models;
using Xamarin.Essentials;

namespace TestProiectLicenta.Data.Services
{
    public class CarService : ICarService
    {
        private readonly HttpClient _client;

        public CarService()
        {
            _client = new HttpClient();
        }

        public async Task<List<Car>> GetCarsAsync()
        {
            var response = await _client.GetAsync(Constants.webAPI + "Cars/All");
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var cars = JsonConvert.DeserializeObject<List<Car>>(content);
            return cars;
        }

        public async Task<Car> GetCar(int carId)
        {
            var url = string.Format(Constants.webAPI + "Cars/{0}", carId);

            if (!CrossConnectivity.Current.IsConnected && !Barrel.Current.IsExpired(url))
                return Barrel.Current.Get<Car>(key: url);
                

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var car = JsonConvert.DeserializeObject<Car>(content);
            Barrel.Current.Add(key: url, data: car, expireIn: TimeSpan.FromDays(1));
            return car;
        }

        public async Task<List<Car>> GetUserCars(int userId)
        {
            var url = string.Format(Constants.webAPI + "Cars/?id={0}", userId);

            if (!CrossConnectivity.Current.IsConnected && !Barrel.Current.IsExpired(url))
                return Barrel.Current.Get<List<Car>>(key: url);
                
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var cars = JsonConvert.DeserializeObject<List<Car>>(content);
            Barrel.Current.Add(key: url, data: cars, expireIn: TimeSpan.FromDays(1));
            return cars;
        }

        public async Task<bool> AddCar(Car car)
        {
            var json = JsonConvert.SerializeObject(car);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(Constants.webAPI + "Cars", content);
            if (response.IsSuccessStatusCode) return true;
            return false;
        }

        public async Task DeleteCar(int id)
        {
            var response = await _client.DeleteAsync(string.Format(Constants.webAPI + "Cars/{0}", id));

            if (response.IsSuccessStatusCode) Debug.WriteLine("Car successfully deleted");
        }

        public async Task<bool> UpdateCar(int carId, Car car)
        {
            var json = JsonConvert.SerializeObject(car);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(string.Format(Constants.webAPI + "Cars?carId={0}", carId), content);
            return response.IsSuccessStatusCode; /*Debug.WriteLine("Car successfully updated");*/
        }

        public async Task AddCarByVin(JObject data, string vin)
        {
            var car = new Car();

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
                car.NumberofDoors = car.NumberofDoors ?? (item["label"].ToString() == "Number of Doors" ? item["value"].ToString() : null);
                car.Steering = car.Steering ?? (item["label"].ToString() == "Steering" ? item["value"].ToString() : null);
                car.Cc = car.Cc ?? (item["label"].ToString() == "Engine Displacement (ccm)" ? item["value"].ToString() : null);
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

            car.Vin = vin;
            var userId = await SecureStorage.GetAsync("UserId");
            car.UserId = Convert.ToInt32(userId);

            var json = JsonConvert.SerializeObject(car);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(Constants.webAPI + "Cars", content);

            if (response.IsSuccessStatusCode) Debug.WriteLine("Car successfully added");
        }
    }
}