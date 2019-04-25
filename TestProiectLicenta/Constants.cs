using System;
namespace TestProiectLicenta
{
    public static class Constants
    {
        public static readonly Uri webAPI = new Uri("https://carmanagementapi20190421042839.azurewebsites.net/api/");

        public static readonly string vinUrl = "https://api.vindecoder.eu/2.0";
        public static readonly string vinApiKey = "b84e3871d2be";
        public static readonly string vinSecretKey = "8d5173a4df";

        public static readonly string imageDetectionAPI = "https://dev.sighthoundapi.com/v1/recognition?objectType=vehicle,licenseplate";
    }
}
