using System;
namespace TestProiectLicenta.Models
{
    public class CarsSold
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public int CarDetails { get; set; }
        public string Details { get; set; }
        public string LongDescription { get; set; }
    }
}
