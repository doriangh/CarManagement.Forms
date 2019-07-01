using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Acr.UserDialogs;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using TestProiectLicenta.Data.Interfaces;
using TestProiectLicenta.Models;
using Xamarin.Essentials;

namespace TestProiectLicenta.Data.Services
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _client;

        public ExternalApiService()
        {
            _client = new HttpClient();
        }

        public async Task<CarVinRequest> GetCarBySelectingPicture()
        {
            var request = new CarVinRequest
            {
                Errors = new List<string>()
            };
             
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                request.Errors.Add("Can't Access Library");
                request.Success = false;
                return request;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions());

            using (UserDialogs.Instance.Loading("Finding out what car you have..."))
            {

                if (file == null)
                {
                    request.Errors.Add("Couldn't read file");
                    request.Success = false;
                    return request;
                }

                var memoryStream = new MemoryStream();
                file.GetStream().CopyTo(memoryStream);

                request.Car = await GetRecognisedCar(memoryStream.ToArray());
                request.Car.CarImage = UploadImageImgur(file);
                request.Success = true;

                File.Delete(file.Path);
            }

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

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            using (UserDialogs.Instance.Loading("Finding out what car you have..."))
            {
                if (file == null)
                {   
                    request.Errors.Add("Couldn't save/find file");
                    request.Success = false;
                    return request;
                }

                var memoryStream = new MemoryStream();
                file.GetStream().CopyTo(memoryStream);

                request.Car = await GetRecognisedCar(memoryStream.ToArray());
                if (request.Car == null)
				{
					request.Success = false;
					request.Errors.Add("Unable to get car");
					return request;
				}
                request.Car.CarImage = UploadImageImgur(file);
                request.Success = true;

                File.Delete(file.Path);
            }

            return request;
        }

        public async Task<CarVinRequest> GetCarByVin(string vin)
        {
            var request = new CarVinRequest
            {
                Errors = new List<string>()
            };

            var id = vin;
            var test = $"{id}|decode|{Constants.vinApiKey}|{Constants.vinSecretKey}";

            var dataHash = Encoding.ASCII.GetBytes(test);

            var hashData = new SHA1Managed().ComputeHash(dataHash);
            var hash = hashData.Aggregate(string.Empty, (current, b) => current + b.ToString("X2"));

            var controlSum = hash.Substring(0, 10);

            var client = new HttpClient();

            var response =
                await client.GetStringAsync($"{Constants.vinUrl}/{Constants.vinApiKey}/{controlSum}/decode/{vin}.json");

            client.Dispose();

            if (response == null)
            {
                request.Errors.Add("Could not get data");
                request.Success = false;
                return request;
            }

            var data = JObject.Parse(response);

            var car = new Car();

            foreach (var item in data["decode"])
            {
                car.Make = car.Make ?? (item["label"].ToString() == "Make" ? item["value"].ToString() : null);
                car.Manufacturer = car.Manufacturer ?? (item["label"].ToString() == "Manufacturer" ? item["value"].ToString() : null);
                car.Plant = car.Plant ?? (item["label"].ToString() == "Manufacturer Address" ? item["value"].ToString() : null);
                car.ModelYear = car.ModelYear ?? (item["label"].ToString() == "Model Year" ? item["value"].ToString() : null);
                car.Model = car.Model ?? (item["label"].ToString() == "Model" ? item["value"].ToString() : null);
                car.Body = car.Body ?? (item["label"].ToString() == "Body" ? item["value"].ToString() : null);
                car.Drive = car.Drive ?? (item["label"].ToString() == "Drive" ? item["value"].ToString() : null);
                car.NumberofSeats = car.NumberofSeats ?? (item["label"].ToString() == "Number of Seats" ? item["value"].ToString() : null);
                car.NumberofDoors = car.NumberofDoors ?? (item["label"].ToString() == "Number of Doors" ? item["value"].ToString() : null);
                car.Steering = car.Steering ?? (item["label"].ToString() == "Steering" ? item["value"].ToString() : null);
                car.Cc = car.Cc ?? (item["label"].ToString() == "Engine Displacement (ccm)" ? item["value"].ToString() : null);
                car.EngineCylinders = car.EngineCylinders ?? (item["label"].ToString() == "Engine Cylinders" ? item["value"].ToString() : null);
                car.Transmission = car.Transmission ?? (item["label"].ToString() == "Transmission" ? item["value"].ToString() : null);
                car.NumberofGears = car.NumberofGears ?? (item["label"].ToString() == "Number of Gears" ? item["value"].ToString() : null);
                car.Color = car.Color ?? (item["label"].ToString() == "Color" ? item["value"].ToString() : null);
                car.Engine = car.Engine ?? (item["label"].ToString() == "Engine (full)" ? item["value"].ToString() : null);
                car.Fuel = car.Fuel ?? (item["label"].ToString() == "Fuel Type - Primary" ? item["value"].ToString() : null);
                car.Power = car.Power ?? (item["label"].ToString() == "Engine Power (kW)" ? item["value"].ToString() : null);
                car.Made = car.Made ?? (item["label"].ToString() == "Made" ? item["value"].ToString() : null);
                car.Emissions = car.Emissions ?? (item["label"].ToString() == "Emission Standard" ? item["value"].ToString() : null);
                //Odometer = data["decode"][2]["value"].ToString()
            }

            car.Vin = vin;
            var userId = await SecureStorage.GetAsync("UserId");
            car.UserId = Convert.ToInt32(userId);

            request.Car = car;
            request.Success = true;
            return request;
        }

        public async Task<CarVinRequest> HandleTakingPicture(CarVinRequest request)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                request.Errors.Add("No camera");
                request.Success = false;
                return request;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            using (UserDialogs.Instance.Loading("Uploading image..."))
            {

                if (file == null)
                {
                    request.Errors.Add("Couldn't save/find file");
                    request.Success = false;
                    return request;
                }

                var memoryStream = new MemoryStream();
                file.GetStream().CopyTo(memoryStream);

                //request.Car = await GetRecognisedCar(memoryStream.ToArray());
                request.Car.CarImage = UploadImageImgur(file);
                request.Success = true;

                File.Delete(file.Path);
            }

            return request;
        }

        public async Task<CarVinRequest> HandleSelectionPicture(CarVinRequest request)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                request.Errors.Add("Can't Access Library");
                request.Success = false;
                return request;
            }

            using (UserDialogs.Instance.Loading("Uploading image..."))
            {
                var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions());

                if (file == null)
                {
                    request.Errors.Add("Couldn't read file");
                    request.Success = false;
                    return request;
                }

                var memoryStream = new MemoryStream();
                file.GetStream().CopyTo(memoryStream);

                //request.Car = await GetRecognisedCar(memoryStream.ToArray());
                request.Car.CarImage = UploadImageImgur(file);
                request.Success = true;

                File.Delete(file.Path);
            }

            return request;
        }

        private static async Task<Car> GetRecognisedCar(byte[] imageStream)
        {
            string response = null;
            Car newCar;

            using (UserDialogs.Instance.Loading("Looking for that image.\nHold on"))
            {
                try
                {
                    var webRequest = (HttpWebRequest)
                        WebRequest.Create(Constants.imageDetectionAPI);
                    webRequest.Method = "POST";
                    webRequest.Accept = "*/*";
                    webRequest.Timeout = 50000;
                    webRequest.KeepAlive = false;
                    webRequest.AllowAutoRedirect = false;
                    webRequest.AllowWriteStreamBuffering = true;
                    webRequest.ContentType = "application/octet-stream";
                    webRequest.Headers["X-Access-Token"] = "ijtnMD6yyOXofoAcLsR1abzUUmDthKwbbbA8";

                    var requestStream = webRequest.GetRequestStream();
                    requestStream.Write(imageStream, 0, imageStream.Length);

                    requestStream.Close();

                    var webResponse = (HttpWebResponse) webRequest.GetResponse();
                    var streamReader =
                        new StreamReader(webResponse.GetResponseStream() ?? throw new InvalidOperationException(),
                            Encoding.UTF8);
                    response = streamReader.ReadToEnd();
                }
                catch (Exception e)
                {
                    Console.Write(e);
                }

                var car = JObject.Parse(response);
                var userId = await SecureStorage.GetAsync("UserId");
				try
				{
					newCar = new Car
					{
						UserId = Convert.ToInt32(userId),
						Make = car["objects"][0]["vehicleAnnotation"]["attributes"]["system"]["make"]["name"].ToString(),
						Model = car["objects"][0]["vehicleAnnotation"]["attributes"]["system"]["model"]["name"].ToString(),
						Color = car["objects"][0]["vehicleAnnotation"]["attributes"]["system"]["color"]["name"].ToString(),
						Body = car["objects"][0]["vehicleAnnotation"]["attributes"]["system"]["vehicleType"].ToString(),
						License =
							car["objects"][0]["vehicleAnnotation"]["licenseplate"]["attributes"]["system"]["string"]["name"]
								.ToString()
					};

					return newCar;

				} catch (ArgumentOutOfRangeException e)
				{
					return null;
				}
            }

            return newCar;
        }

        public string UploadImageImgur(MediaFile file)
        {
            using (var w = new WebClient())
            {
                w.Headers.Add("Authorization", "Client-ID " + Constants.imgurId);
                var values = new NameValueCollection
                {
                    {"image", Convert.ToBase64String(File.ReadAllBytes(file.Path))}
                };

                var response = w.UploadValues(Constants.imgurUrl, values);
                var xml = XDocument.Load(new MemoryStream(response));

                return xml.Root.Element("link").Value;
            }
        }
    }
}