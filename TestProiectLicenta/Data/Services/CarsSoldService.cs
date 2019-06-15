using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

        public async Task<List<CarsSold>> GetAll()
        {
            var response = await _client.GetAsync(Constants.webAPI + "CarsSold");
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var cars = JsonConvert.DeserializeObject<List<CarsSold>>(content);
            return cars;
        }

        public async Task<CarsSold> GetById(int id)
        {
            var url = string.Format(Constants.webAPI + "CarsSold/{0}", id);

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var car = JsonConvert.DeserializeObject<CarsSold>(content);
            return car;
        }

        public async Task<List<CarsSold>> GetByUserId(int userId)
        {
            var url = string.Format(Constants.webAPI + "CarsSold/User/{0}", userId);

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var car = JsonConvert.DeserializeObject<List<CarsSold>>(content);
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
