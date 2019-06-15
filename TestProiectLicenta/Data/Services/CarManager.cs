﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TestProiectLicenta.Data.Interfaces;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Services
{
    public class CarManager
    {
        private readonly ICarService _service;

        public CarManager(ICarService service)
        {
            _service = service;
        }

        public Task<List<Car>> GetCarsAsync()
        {
            return _service.GetCarsAsync();
        }

        public Task<Car> GetCar(int carId)
        {
            return _service.GetCar(carId);
        }

        public Task<List<Car>> GetUserCars(int userId)
        {
            return _service.GetUserCars(userId);
        }

        public Task<bool> AddCar(Car car)
        {
            return _service.AddCar(car);
        }

        public Task DeleteCar(int id)
        {
            return _service.DeleteCar(id);
        }

        public Task<bool> UpdateCar(int id, Car car)
        {
            return _service.UpdateCar(id, car);
        }

        public Task AddCarByVin(JObject data, string vin)
        {
            return _service.AddCarByVin(data, vin);
        }
    }
}