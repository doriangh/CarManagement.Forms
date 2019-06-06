using System.Collections.Generic;

namespace TestProiectLicenta.Models
{
    public class CarPriceResponse
    {
        public float Price { get; set; }
        public List<string> Errors { get; set; }
        public bool Success { get; set; }
    }
}