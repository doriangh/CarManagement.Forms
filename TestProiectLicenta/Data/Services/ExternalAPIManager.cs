using System.Threading.Tasks;
using TestProiectLicenta.Data.Interfaces;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Services
{
    public class ExternalApiManager
    {
        private readonly IExternalApiService _service;

        public ExternalApiManager(IExternalApiService service)
        {
            _service = service;
        }

        public Task<CarVinRequest> GetCarByVin(string vin)
        {
            return _service.GetCarByVin(vin);
        }

        public Task<CarVinRequest> GetCarByTakingPictureAsync()
        {
            return _service.GetCarByTakingPictureAsync();
        }

        public Task<CarVinRequest> GetCarBySelectingPicture()
        {
            return _service.GetCarBySelectingPicture();
        }

        public Task<CarVinRequest> HandleSelectionPicture(CarVinRequest request)
        {
            return _service.HandleSelectionPicture(request);
        }

        public Task<CarVinRequest> HandleTakingPicture(CarVinRequest request)
        {
            return _service.HandleTakingPicture(request);
        }
    }
}