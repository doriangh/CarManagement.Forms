using System.Collections.Generic;
using System.Threading.Tasks;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Interfaces
{
    public interface ICarImageService
    {
        Task<List<CarImages>> GetAllCarImages(bool force = false);
        Task<CarImages> GetCarImage(int id, bool force = false);
        Task<List<CarImages>> GetCarsImages(int carId, bool force = false);
        Task UpdateCarImages(CarImages carImages);
        Task AddCarImages(CarImages images);
        Task DeleteCarImages(int id);
    }
}
