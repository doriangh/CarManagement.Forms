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
    public class CarDetailService : ICarDetailService
    {
        HttpClient _client;

        public CarDetailService()
        {
            _client = new HttpClient();
        }

        public async Task AddCarDetail(CarDetail carDetail)
        {
            var json = JsonConvert.SerializeObject(carDetail);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await _client.PostAsync(Constants.webAPI + "CarDetails", content);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Car successfully saved");
            }
        }

        public async Task DeleteCarDetail(int id)
        {
            var response = await _client.DeleteAsync(string.Format(Constants.webAPI + "CarDetails/{0}", id));

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Car successfully deleted");
            }
        }

        public async Task<CarDetail> GetCarDetail(int carDetailId)
        {
            var response = await _client.GetAsync(string.Format(Constants.webAPI + "CarDetails/{0}", carDetailId));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var car = JsonConvert.DeserializeObject<CarDetail>(content);
                return car;
            }
            return null;
        }

        public async Task<List<CarDetail>> GetCarDetails()
        {
            var response = await _client.GetAsync(Constants.webAPI + "CarDetails");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cars = JsonConvert.DeserializeObject<List<CarDetail>>(content);
                return cars;
            }
            return null;
        }

        public async Task<CarDetail> GetCarsDetail(int carId)
        {
            var response = await _client.GetAsync(string.Format(Constants.webAPI + "CarDetails/Car/{0}", carId));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cars = JsonConvert.DeserializeObject<CarDetail>(content);
                return cars;
            }
            return null;
        }

        public async Task UpdateCarDetail(CarDetail carDetail)
        {
            var json = JsonConvert.SerializeObject(carDetail);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await _client.PutAsync(Constants.webAPI + "Cars", content);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Car successfully updated");
            }
        }
    }
}
