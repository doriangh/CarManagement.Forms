using System.Collections.Generic;
using System.Threading.Tasks;
using TestProiectLicenta.Data.Interfaces;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Services
{
    public class CarDetailManager
    {
        private readonly ICarDetailService _service;

        public CarDetailManager(ICarDetailService service)
        {
            _service = service;
        }

        public Task<List<CarDetail>> GetCarDetails(bool force = false)
        {
            return _service.GetCarDetails(force);
        }

        public Task<CarDetail> GetCarDetail(int carDetailId, bool force = false)
        {
            return _service.GetCarDetail(carDetailId, force);
        }

        public Task<CarDetail> GetCarsDetail(int carId, bool force = false)
        {
            return _service.GetCarsDetail(carId, force);
        }

        public Task AddCarDetail(CarDetail carDetail)
        {
            return _service.AddCarDetail(carDetail);
        }

        public Task DeleteCarDetail(int id)
        {
            return _service.DeleteCarDetail(id);
        }

        public Task UpdateCarDetail(CarDetail carDetail)
        {
            return _service.UpdateCarDetail(carDetail);
        }
    }
}