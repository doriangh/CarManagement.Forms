﻿using System;
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
    public class CarDetailService : ICarDetailService
    {
        private readonly HttpClient _client;

        public CarDetailService()
        {
            _client = new HttpClient();
        }

        public async Task AddCarDetail(CarDetail carDetail)
        {
            var json = JsonConvert.SerializeObject(carDetail);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(Constants.webAPI + "CarDetails", content);
            if (response.IsSuccessStatusCode) Debug.WriteLine("Car successfully saved");
        }

        public async Task DeleteCarDetail(int id)
        {
            var response = await _client.DeleteAsync(string.Format(Constants.webAPI + "CarDetails/{0}", id));

            if (response.IsSuccessStatusCode) Debug.WriteLine("Car successfully deleted");
        }

        public async Task<CarDetail> GetCarDetail(int carDetailId)
        {
            var url = string.Format(Constants.webAPI + "CarDetails/{0}", carDetailId);

            if (!CrossConnectivity.Current.IsConnected && !Barrel.Current.IsExpired(url))
                return Barrel.Current.Get<CarDetail>(key: url);

            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var car = JsonConvert.DeserializeObject<CarDetail>(content);
            Barrel.Current.Add(key: url, data: car, expireIn: TimeSpan.FromDays(1));
            return car;
        }

        public async Task<List<CarDetail>> GetCarDetails()
        {
            var url = Constants.webAPI + "CarDetails";

            if (!CrossConnectivity.Current.IsConnected && !Barrel.Current.IsExpired(url))
                return Barrel.Current.Get<List<CarDetail>>(key: url);
               
            var response = await _client.GetAsync(Constants.webAPI + "CarDetails");
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            var cars = JsonConvert.DeserializeObject<List<CarDetail>>(content);
            Barrel.Current.Add(key: url, data: cars, expireIn: TimeSpan.FromDays(1));
            return cars;
        }

        public async Task<CarDetail> GetCarsDetail(int carId)
        {
            //var response = await _client.GetAsync(string.Format(Constants.webAPI + "CarDetails/Car/{0}", carId));
            //if (response.IsSuccessStatusCode)
            //{

            //    var content = await response.Content.ReadAsStringAsync();
            //    if (content != "[]")
            //    {
            //        var cars = JsonConvert.DeserializeObject<CarDetail>(content);
            //        return cars;
            //    }
            //    return null;
            //}
            //return null;

            var allCarDetails = await GetCarDetails();

            foreach (var carDetail in allCarDetails)
                if (carDetail.CarId == carId)
                    return carDetail;

            return null;
        }

        public async Task UpdateCarDetail(CarDetail carDetail)
        {
            var json = JsonConvert.SerializeObject(carDetail);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(Constants.webAPI + "Cars", content);
            if (response.IsSuccessStatusCode) Debug.WriteLine("Car successfully updated");
        }
    }
}