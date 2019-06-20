using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestProiectLicenta.Data.Interfaces;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Services
{
    public class CarImagesManager
    {
        private ICarImageService _service;

        public CarImagesManager(ICarImageService service)
        {
            _service = service;
        }

        public Task<List<CarImages>> GetCarImages(bool force = false)
        {
            return _service.GetAllCarImages(force);
        }

        public Task<CarImages> GetCarImage(int id, bool force = false)
        {
            return _service.GetCarImage(id, force);
        }

        public Task<List<CarImages>> GetCarsImages(int carId, bool force = false)
        {
            return _service.GetCarsImages(carId, force);
        }

        public void UpdateCarImages(CarImages carImages)
        {
            _service.UpdateCarImages(carImages);
        }

        public Task AddCarImages(CarImages images)
        {
            return _service.AddCarImages(images);
        }

        public Task DeleteCarImages(int id)
        {
            return _service.DeleteCarImages(id);
        }
    }
}
