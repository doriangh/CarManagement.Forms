using System;
namespace TestProiectLicenta.Models
{
    public class CarPriceRequest
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Cc { get; set; }
        public int Odometer { get; set; }
    }
}
