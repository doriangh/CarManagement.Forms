using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Interfaces
{
    public interface ICarImageService
    {
        Task<List<CarImages>> GetAllCarImages();
        Task<CarImages> GetCarImage(int id);
        Task<List<CarImages>> GetCarsImages(int carId);
        Task UpdateCarImages(CarImages carImages);
        Task AddCarImages(CarImages images);
        Task DeleteCarImages(int id);
    }
}
