using System;
using System.Threading.Tasks;
using TestProiectLicenta.Data.Interfaces;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Services
{
    public class GetCarPriceManager
    {
        private readonly IGetCarPriceService _service;

        public GetCarPriceManager(IGetCarPriceService service)
        {
            _service = service;
        }

        public async Task<CarPriceResponse> GetCarPrice(CarPriceRequest car)
        {
            return await _service.GetCarPrice(car);
        }
    }
}
