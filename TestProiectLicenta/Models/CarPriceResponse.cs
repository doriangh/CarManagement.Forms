using System;
using System.Collections.Generic;

namespace TestProiectLicenta.Models
{
    public class CarPriceResponse
    {
        public int Price { get; set; }
        public int Count { get; set; }
        public List<string> Errors { get; set; }
        public bool Success { get; set; }
    }
}