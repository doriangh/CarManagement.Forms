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
    public class CarsSoldService : ICarsSoldService
    {
        private HttpClient _client;

        public CarsSoldService ()
        {
            _client = new HttpClient();
        }

        public async Task Add(CarsSold carsSold)
        {
            var json = JsonConvert.SerializeObject(carsSold);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(Constants.webAPI + "CarsSold", content);
        }

        public async Task Delete(int id)
        {
            var response = await _client.DeleteAsync(string.Format(Constants.webAPI + "CarsSold/{0}", id));

            if (response.IsSuccessStatusCode) Debug.WriteLine("Car successfully deleted");
        }

        public async Task<List<CarsSold>> GetAll(bool force = false)
        {
            var url = Constants.webAPI + "CarsSold";

            if (!CrossConnectivity.Current.IsConnected)
                return Barrel.Current.Get<List<CarsSold>>(url);

            if (!force && !Barrel.Current.IsExpired(url))
                return Barrel.Current.Get<List<CarsSold>>(url);

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var cars = JsonConvert.DeserializeObject<List<CarsSold>>(content);
            Barrel.Current.Add(url, cars, TimeSpan.FromDays(7));
            return cars;
        }

        public async Task<CarsSold> GetByCarId(int carId, bool force = false)
        {
            var url = string.Format(Constants.webAPI + "CarsSold/Cars/{0}", carId);

            if (!CrossConnectivity.Current.IsConnected)
                return Barrel.Current.Get<CarsSold>(url);

            if (!force && !Barrel.Current.IsExpired(url))
                return Barrel.Current.Get<CarsSold>(url);

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var car = JsonConvert.DeserializeObject<CarsSold>(content);
            Barrel.Current.Add(url, car, TimeSpan.FromDays(7));
            return car;
        }

        public async Task<CarsSold> GetById(int id, bool force = false)
        {
            var url = string.Format(Constants.webAPI + "CarsSold/{0}", id);

            if (!CrossConnectivity.Current.IsConnected)
                return Barrel.Current.Get<CarsSold>(url);

            if (!force && !Barrel.Current.IsExpired(url))
                return Barrel.Current.Get<CarsSold>(url);

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var car = JsonConvert.DeserializeObject<CarsSold>(content);
            Barrel.Current.Add(url, car, TimeSpan.FromDays(7));
            return car;
        }

        public async Task<List<CarsSold>> GetByUserId(int userId, bool force = false)
        {
            var url = string.Format(Constants.webAPI + "CarsSold/User/{0}", userId);

            if (!CrossConnectivity.Current.IsConnected)
                return Barrel.Current.Get<List<CarsSold>>(url);

            if (!force && !Barrel.Current.IsExpired(url))
                return Barrel.Current.Get<List<CarsSold>>(url);

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var car = JsonConvert.DeserializeObject<List<CarsSold>>(content);
            Barrel.Current.Add(url, car, TimeSpan.FromDays(7));
            return car;
        }

        public async Task Update(CarsSold carsSold)
        {
            var json = JsonConvert.SerializeObject(carsSold);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _client.PutAsync(string.Format(Constants.webAPI + "CarsSold"), content);
        }
    }
}
