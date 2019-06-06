using System;

namespace TestProiectLicenta
{
    public static class Constants
    {
        //public static readonly Uri webAPI = new Uri("https://carmanagementapi20190421042839.azurewebsites.net/api/");
        //public static readonly Uri webAPI = new Uri("https://7cc962a6.ngrok.io/api/");
        public static readonly Uri webAPI = new Uri("https://carmanagementapi.appspot.com/api/");
        //public static readonly Uri webAPI = new Uri("https://indux.serveo.net/api/");

        public const string vinUrl = "https://api.vindecoder.eu/3.0";
        public const string vinApiKey = "b84e3871d2be";
        public const string vinSecretKey = "8d5173a4df";

        public const string imageDetectionAPI = "https://dev.sighthoundapi.com/v1/recognition?objectType=vehicle,licenseplate";

        public static readonly string imgurId = "3998115b75eb6f3";
        public static readonly string imgurSecret = "17246fb9c2e052d96773af41fdf5091b7ba71603";
        public static readonly string imgurUrl = "https://api.imgur.com/3/upload.xml";

        public static readonly string url = "http://www.carimagery.com/api.asmx/GetImageUrl?searchTerm=";

        public static readonly string fuelAPIKey = "daefd14b-9f2b-4968-9e4d-9d4bb4af01d1";
    }
}