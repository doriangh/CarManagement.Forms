using System.Threading.Tasks;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Interfaces
{
    public interface IGetCarPriceService
    {
        Task<CarPriceResponse> GetCarPrice(CarPriceRequest car);
    }
}