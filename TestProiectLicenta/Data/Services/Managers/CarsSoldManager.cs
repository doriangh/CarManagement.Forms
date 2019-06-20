using System.Collections.Generic;
using System.Threading.Tasks;
using TestProiectLicenta.Data.Interfaces;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Services
{
    public class CarsSoldManager
    {
        private ICarsSoldService _service;

        public CarsSoldManager(ICarsSoldService service)
        {
            _service = service;
        }

        public Task<List<CarsSold>> GetAll(bool force = false)
        {
            return _service.GetAll(force);
        }
        public Task<CarsSold> GetById(int id, bool force = false)
        {
            return _service.GetById(id, force);
        }
        public Task Update(CarsSold carsSold)
        {
            return _service.Update(carsSold);
        }
        public Task<List<CarsSold>> GetByUserId(int userId, bool force = false)
        {
            return _service.GetByUserId(userId, force);
        }
        public Task Add(CarsSold carsSold)
        {
            return _service.Add(carsSold);
        }
        public Task Delete(int id)
        {
            return _service.Delete(id);
        }
        public Task<CarsSold> GetByCarId(int carId, bool force = false)
        {
            return _service.GetByCarId(carId, force);
        }
    }
}
