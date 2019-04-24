using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using TestProiectLicenta.Data.Interfacess;
using TestProiectLicenta.Models;
using Xamarin.Essentials;

namespace TestProiectLicenta.Data.Services
{
    public class ExternalAPIService : IExternalAPIService
    {
        readonly HttpClient _client;

        public ExternalAPIService()
        {
            _client = new HttpClient();
        }

        public async Task<CarVinRequest> GetCarBySelectingPicture()
        {
            await CrossMedia.Current.Initialize();

            var request = new CarVinRequest
            {
                Errors = new List<string>()
            };

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                request.Errors.Add("Can't Access Library");
                request.Success = false;
                return request;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions { });

            if (file == null)
            {
                request.Errors.Add("Couldn't read file");
                request.Success = false;
                return request;
            }

            var memoryStream = new MemoryStream();
            file.GetStream().CopyTo(memoryStream);

            File.Delete(file.Path);

            request.Car = await GetRecognisedCar(memoryStream.ToArray());
            request.Success = true;

            return request;

        }

        public async Task<CarVinRequest> GetCarByTakingPictureAsync()
        {
            var request = new CarVinRequest
            {
                Errors = new List<string>()
            };

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                request.Errors.Add("No camera");
                request.Success = false;
                return request;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
            {
                request.Errors.Add("Couldn't save/find file");
                request.Success = false;
                return request;
            }

            var memoryStream = new MemoryStream();
            file.GetStream().CopyTo(memoryStream);

            File.Delete(file.Path);

            request.Car = await GetRecognisedCar(memoryStream.ToArray());
            request.Success = true;

            return request;

        }

        public async Task<JObject> GetCarByVIN(string VIN)
        {
            string id = VIN;
            string test = String.Format("{0}|{1}|{2}", id, Constants.vinApiKey, Constants.vinSecretKey);

            var dataHash = Encoding.ASCII.GetBytes(test);

            var hashData = new SHA1Managed().ComputeHash(dataHash);
            var hash = string.Empty;

            foreach (var b in hashData)
            {
                hash += b.ToString("X2");
            }

            var controlSum = hash.Substring(0, 10);

            var client = new HttpClient();

            var response = await client.GetStringAsync(string.Format("{0}/{1}/{2}/decode/{3}.json", Constants.vinUrl, Constants.vinApiKey, controlSum, VIN));

            client.Dispose();

            return JObject.Parse(response);

        }

        public async Task<Car> GetRecognisedCar(byte[] imageStream)
        {
            string Response = null;
            HttpWebRequest webRequest = null;
            HttpWebResponse webResponse = null;
            StreamReader streamReader = null;
            Stream requestStream = null;

            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(Constants.imageDetectionAPI);
                webRequest.Method = "POST";
                webRequest.Accept = "*/*";
                webRequest.Timeout = 50000;
                webRequest.KeepAlive = false;
                webRequest.AllowAutoRedirect = false;
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.ContentType = "application/octet-stream";
                webRequest.Headers["X-Access-Token"] = "ijtnMD6yyOXofoAcLsR1abzUUmDthKwbbbA8";

                requestStream = webRequest.GetRequestStream();
                requestStream.Write(imageStream, 0, imageStream.Length);

                requestStream.Close();

                webResponse = (HttpWebResponse)webRequest.GetResponse();
                streamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
                Response = streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                Console.Write(e);
            }

            var car = JObject.Parse(Response);
            var userId = await SecureStorage.GetAsync("UserId");

            Car newCar = new Car()
            {
                UserId = Convert.ToInt32(userId),
                Make = car["objects"][0]["vehicleAnnotation"]["attributes"]["system"]["make"]["name"].ToString(),
                Model = car["objects"][0]["vehicleAnnotation"]["attributes"]["system"]["model"]["name"].ToString(),
                Color = car["objects"][0]["vehicleAnnotation"]["attributes"]["system"]["color"]["name"].ToString(),
                Body = car["objects"][0]["vehicleAnnotation"]["attributes"]["system"]["vehicleType"].ToString(),
                License = car["objects"][0]["vehicleAnnotation"]["licenseplate"]["attributes"]["system"]["string"]["name"].ToString()
            };

            return newCar;
        }

    }
}
