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

        public Task<List<CarDetail>> GetCarDetails()
        {
            return _service.GetCarDetails();
        }

        public Task<CarDetail> GetCarDetail(int carDetailId)
        {
            return _service.GetCarDetail(carDetailId);
        }

        public Task<CarDetail> GetCarsDetail(int carId)
        {
            return _service.GetCarsDetail(carId);
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
