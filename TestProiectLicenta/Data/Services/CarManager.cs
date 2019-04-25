using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TestProiectLicenta.Interfaces.Services;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Services
{
    public class CarManager
    {
        readonly ICarService _service;

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

        public Task AddCar(Car car)
        {
            return _service.AddCar(car);
        }

        public Task DeleteCar(int id)
        {
            return _service.DeleteCar(id);
        }

        public Task UpdateCar(Car car)
        {
            return _service.UpdateCar(car);
        }

        public Task AddCarByVIN(JObject data, string VIN)
        {
            return _service.AddCarByVIN(data, VIN);
        }
    }
}
