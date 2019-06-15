using System;
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

        public Task<List<CarsSold>> GetAll()
        {
            return _service.GetAll();
        }
        public Task<CarsSold> GetById(int id)
        {
            return _service.GetById(id);
        }
        public Task Update(CarsSold carsSold)
        {
            return _service.Update(carsSold);
        }
        public Task<List<CarsSold>> GetByUserId(int userId)
        {
            return _service.GetByUserId(userId);
        }
        public Task Add(CarsSold carsSold)
        {
            return _service.Add(carsSold);
        }
        public Task Delete(int id)
        {
            return _service.Delete(id);
        }
    }
}
