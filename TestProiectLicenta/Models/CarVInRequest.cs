using System;
using System.Collections.Generic;

namespace TestProiectLicenta.Models
{
    public class CarVinRequest
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public Car Car { get; set; }
    }
}
