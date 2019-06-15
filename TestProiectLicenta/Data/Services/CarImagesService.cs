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

            var response = await _client.PostAsync(Constants.webAPI + "CarImages", content);
        }

        public async Task DeleteCarImages(int id)
        {
            var response = await _client.DeleteAsync(string.Format(Constants.webAPI + "CarImage/{0}", id));

            if (response.IsSuccessStatusCode) Debug.WriteLine("Car successfully deleted");
        }

        public async Task<CarImages> GetCarImage(int id)
        {
            var url = string.Format(Constants.webAPI + "CarImages/{0}", id);

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var car = JsonConvert.DeserializeObject<CarImages>(content);
            return car;
        }

        public async Task<List<CarImages>> GetAllCarImages()
        {
            var url = string.Format(Constants.webAPI + "CarImages/All");

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var car = JsonConvert.DeserializeObject<List<CarImages>>(content);
            return car;
        }

        public async Task<List<CarImages>> GetCarsImages(int carId)
        {
            var url = string.Format(Constants.webAPI + "CarImages/?carId={0}", carId);

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var cars = JsonConvert.DeserializeObject<List<CarImages>>(content);
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
