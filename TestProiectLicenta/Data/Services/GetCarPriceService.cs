using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestProiectLicenta.Data.Interfaces;
using TestProiectLicenta.Models;

namespace TestProiectLicenta.Data.Services
{
    public class GetCarPriceService : IGetCarPriceService
    {
        private readonly HttpClient _client;

        public GetCarPriceService()
        {
            _client = new HttpClient();
        }

        public async Task<CarPriceResponse> GetCarPrice(CarPriceRequest car)
        {
            var json = JsonConvert.SerializeObject(car);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(Constants.webAPI + "CarPrice", content);
            if (!response.IsSuccessStatusCode) return null;
            var recvContent = await response.Content.ReadAsStringAsync();
            var price = JsonConvert.DeserializeObject<CarPriceResponse>(recvContent);
            return price;

        }
    }
}
