using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TestProiectLicenta.Data.Interfacess;
using TestProiectLicenta.Interfaces.Services;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Services
{
    public class ExternalAPIManager
    {
        IExternalAPIService _service;

        public ExternalAPIManager(IExternalAPIService service)
        {
            _service = service;
        }

        public Task<JObject> GetCarByVIN(string VIN)
        {
            return _service.GetCarByVIN(VIN);
        }

        public Task<CarVinRequest> GetCarByTakingPictureAsync()
        {
            return _service.GetCarByTakingPictureAsync();
        }

        public Task<CarVinRequest> GetCarBySelectingPicture()
        {
            return _service.GetCarBySelectingPicture();
        }
    }
}
