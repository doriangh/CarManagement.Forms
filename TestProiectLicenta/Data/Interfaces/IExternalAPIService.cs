using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Interfacess
{
    public interface IExternalAPIService
    {
        Task<JObject> GetCarByVIN(string VIN);
        Task<CarVinRequest> GetCarBySelectingPicture();
        Task<CarVinRequest> GetCarByTakingPictureAsync();
        Task<CarVinRequest> HandleSelectionPicture(CarVinRequest request);
        Task<CarVinRequest> HandleTakingPicture(CarVinRequest request);
    }
}
