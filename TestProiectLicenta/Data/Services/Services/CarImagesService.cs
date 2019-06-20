using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MonkeyCache.FileStore;
using Newtonsoft.Json;
using Plugin.Connectivity;
using TestProiectLicenta.Data.Interfaces;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Services
{
    public class CarImagesService : ICarImageService
    {
        private readonly HttpClient _client;

        public CarImagesService()
        {
            _client = new HttpClient();
        }

        public async Task AddCarImages(CarImages images)
        {
            var json = JsonConvert.SerializeObject(images);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _client.PostAsync(Constants.webAPI + "CarImages", content);
        }

        public async Task DeleteCarImages(int id)
        {
            var response = await _client.DeleteAsync(string.Format(Constants.webAPI + "CarImage/{0}", id));

            if (response.IsSuccessStatusCode) Debug.WriteLine("Car successfully deleted");
        }

        public async Task<CarImages> GetCarImage(int id, bool force = false)
        {
            var url = string.Format(Constants.webAPI + "CarImages/{0}", id);

            if (!CrossConnectivity.Current.IsConnected)
                return Barrel.Current.Get<CarImages>(url);

            if (!force && !Barrel.Current.IsExpired(url))
                return Barrel.Current.Get<CarImages>(url);

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var car = JsonConvert.DeserializeObject<CarImages>(content);
            Barrel.Current.Add(url, car, TimeSpan.FromDays(7));
            return car;
        }

        public async Task<List<CarImages>> GetAllCarImages(bool force = false)
        {
            var url = string.Format(Constants.webAPI + "CarImages/All");

            if (!CrossConnectivity.Current.IsConnected)
                return Barrel.Current.Get<List<CarImages>>(url);

            if (!force && !Barrel.Current.IsExpired(url))
                return Barrel.Current.Get<List<CarImages>>(url);

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var car = JsonConvert.DeserializeObject<List<CarImages>>(content);
            Barrel.Current.Add(url, car, TimeSpan.FromDays(7));
            return car;
        }

        public async Task<List<CarImages>> GetCarsImages(int carId, bool force = false)
        {
            var url = string.Format(Constants.webAPI + "CarImages/?carId={0}", carId);

            if (!CrossConnectivity.Current.IsConnected)
                return Barrel.Current.Get<List<CarImages>>(url);

            if (!force && !Barrel.Current.IsExpired(url))
                return Barrel.Current.Get<List<CarImages>>(url);

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var cars = JsonConvert.DeserializeObject<List<CarImages>>(content);
            Barrel.Current.Add(url, cars, TimeSpan.FromDays(7));
            return cars;
        }

        public async Task UpdateCarImages(CarImages carImages)
        {
            var json = JsonConvert.SerializeObject(carImages);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(Constants.webAPI + "CarImages", content);
            if (response.IsSuccessStatusCode) Debug.WriteLine("Car successfully updated");
        }
    }
}
