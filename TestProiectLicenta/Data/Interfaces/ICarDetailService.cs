using System.Collections.Generic;
using System.Threading.Tasks;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Interfaces
{
    public interface ICarDetailService
    {
        Task<List<CarDetail>> GetCarDetails(bool force = false);
        Task<CarDetail> GetCarDetail(int carDetailId, bool force = false);
        Task<CarDetail> GetCarsDetail(int carId, bool force = false);
        Task AddCarDetail(CarDetail carDetail);
        Task DeleteCarDetail(int id);
        Task UpdateCarDetail(CarDetail carDetail);
    }
}