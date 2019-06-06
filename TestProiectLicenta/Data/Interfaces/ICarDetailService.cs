using System.Collections.Generic;
using System.Threading.Tasks;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Interfaces
{
    public interface ICarDetailService
    {
        Task<List<CarDetail>> GetCarDetails();
        Task<CarDetail> GetCarDetail(int carDetailId);
        Task<CarDetail> GetCarsDetail(int carId);
        Task AddCarDetail(CarDetail carDetail);
        Task DeleteCarDetail(int id);
        Task UpdateCarDetail(CarDetail carDetail);
    }
}