using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Interfaces
{
    public interface IExternalApiService
    {
        Task<CarVinRequest> GetCarByVin(string vin);
        Task<CarVinRequest> GetCarBySelectingPicture();
        Task<CarVinRequest> GetCarByTakingPictureAsync(MediaFile file);
        Task<CarVinRequest> HandleSelectionPicture(CarVinRequest request);
        Task<CarVinRequest> HandleTakingPicture(CarVinRequest request);
        string UploadImageImgur(MediaFile file);
    }
}