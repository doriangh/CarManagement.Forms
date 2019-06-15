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

        public Task<List<CarImages>> GetCarImages()
        {
            return _service.GetAllCarImages();
        }

        public Task<CarImages> GetCarImage(int id)
        {
            return _service.GetCarImage(id);
        }

        public Task<List<CarImages>> GetCarsImages(int carId)
        {
            return _service.GetCarsImages(carId);
        }

        public void UpdateCarImages(CarImages carImages)
        {
            _service.UpdateCarImages(carImages);
        }

        public void AddCarImages(CarImages images)
        {
            _service.AddCarImages(images);
        }

        public void DeleteCarImages(int id)
        {
            _service.DeleteCarImages(id);
        }
    }
}
