using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TestProiectLicenta.Logic
{
    public class GetCarByVin
    {
        public JObject carData;
        readonly string url = "https://api.vindecoder.eu/2.0";
        readonly string apiKey = "b84e3871d2be";
        readonly string secretKey = "8d5173a4df";

        public async Task<string> GetCar(string VIN)
        {
            string id = VIN;
            string test = String.Format("{0}|{1}|{2}", id, apiKey, secretKey);

            var dataHash = Encoding.ASCII.GetBytes(test);

            var hashData = new SHA1Managed().ComputeHash(dataHash);
            var hash = string.Empty;

            foreach (var b in hashData)
            {
                hash += b.ToString("X2");
            }

            var controlSum = hash.Substring(0, 10);

            var client = new HttpClient();

            var response = await client.GetStringAsync(string.Format("{0}/{1}/{2}/decode/{3}.json", url, apiKey, controlSum, VIN));

            client.Dispose();

            return response;
        }
    }
}
