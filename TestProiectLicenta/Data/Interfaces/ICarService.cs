using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Interfaces
{
    public interface ICarService
    {
        Task<List<Car>> GetCarsAsync();
        Task<Car> GetCar(int carId);
        Task<List<Car>> GetUserCars(int userId);
        Task AddCar(Car car);
        Task DeleteCar(int id);
        Task<bool> UpdateCar(int id, Car car);
        Task AddCarByVin(JObject data, string vin);
    }
}