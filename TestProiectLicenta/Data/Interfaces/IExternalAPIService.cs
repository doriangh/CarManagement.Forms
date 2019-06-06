using System.Threading.Tasks;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Interfaces
{
    public interface IExternalApiService
    {
        Task<CarVinRequest> GetCarByVin(string vin);
        Task<CarVinRequest> GetCarBySelectingPicture();
        Task<CarVinRequest> GetCarByTakingPictureAsync();
        Task<CarVinRequest> HandleSelectionPicture(CarVinRequest request);
        Task<CarVinRequest> HandleTakingPicture(CarVinRequest request);
    }
}