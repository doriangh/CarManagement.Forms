using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Interfaces
{
    public interface ICarService
    {
        Task<List<Car>> GetCarsAsync(bool force = false);
        Task<Car> GetCar(int carId, bool force = false);
        Task<List<Car>> GetUserCars(int userId, bool force = false);
        Task<bool> AddCar(Car car);
        Task DeleteCar(int id);
        Task<bool> UpdateCar(int id, Car car);
        Task AddCarByVin(JObject data, string vin);
    }
}